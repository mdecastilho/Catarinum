using System.Collections.Generic;

namespace Catarinum.Coap.Layers {
    public class TransferLayer : UpperLayer {
        public const int DefaultBlockSize = 512;
        private readonly TokenManager _tokenManager;
        private readonly Dictionary<string, Message> _incomplete;
        private readonly int _szx;

        public TransferLayer(ILayer lowerLayer)
            : this(lowerLayer, DefaultBlockSize) {
        }

        public TransferLayer(ILayer lowerLayer, int blockSize)
            : base(lowerLayer) {
            _tokenManager = new TokenManager();
            _incomplete = new Dictionary<string, Message>();

            if (blockSize > 0) {
                _szx = BlockOption.EncodeSzx(blockSize);
            }
        }

        public override void Send(Message message) {
            var num = 0;
            var szx = _szx;

            // block size negotiation
            if (message is Response) {
                var buddyBlock = (BlockOption) ((Response) message).Request.GetFirstOption(OptionNumber.Block2);

                if (buddyBlock != null) {
                    if (buddyBlock.Szx < _szx) {
                        szx = buddyBlock.Szx;
                    }

                    num = buddyBlock.Num;
                }
            }

            if (message.Payload.Length > BlockOption.DecodeSzx(szx)) {
                var block = message.GetBlock(num, szx);

                if (block != null) {
                    block.Token = _tokenManager.AcquireToken();
                    SendMessageOverLowerLayer(block);

                    if (((BlockOption) block.GetFirstOption(OptionNumber.Block2)).M > 0) {
                        _incomplete.Add(message.GetTransactionKey(), message);
                    }
                }
                else {
                    return;
                    // handleOutOfScopeError(msg);
                }
            }

            SendMessageOverLowerLayer(message);
        }

        public override void OnReceive(Message message) {
            var block1 = message.GetFirstOption(OptionNumber.Block1) as BlockOption;
            var block2 = message.GetFirstOption(OptionNumber.Block2) as BlockOption;

            if (block1 == null && block2 == null) {
                base.OnReceive(message);
            }
            else if (message is Request && block2 != null) {
                // send blockwise response

                if (!_incomplete.ContainsKey(message.GetTransactionKey())) {
                    base.OnReceive(message);
                }
                else {
                    var first = _incomplete[message.GetTransactionKey()];
                    var block = first.GetBlock(block2.Num, block2.Szx);

                    if (block != null) {
                        block.Id = message.Id;
                        var m = ((BlockOption) block.GetFirstOption(OptionNumber.Block2)).M;
                        SendMessageOverLowerLayer(block);

                        if (m == 0) {
                            _incomplete.Remove(message.GetTransactionKey());
                        }
                    }
                }
            }
            else if (message is Response && block1 != null) {
                // handle blockwise acknowledgement
            }
            else if (message is Response && block2 != null) {
                // handle incoming payload using block2
                HandleIncomingPayload(message, block2);
            }
        }

        private void HandleIncomingPayload(Message message, BlockOption blockOption) {
            var key = message.GetTransactionKey();

            if (_incomplete.ContainsKey(key)) {
                _incomplete[key].Id = message.Id;
                _incomplete[key].AppendPayload(message.Payload);
                _incomplete[key].SetOption(blockOption);
            }
            else if (blockOption.Num == 0) {
                _incomplete.Add(key, message);
            }
            else {
                //Log.error(this, "Transfer started out of order: %s", msg.key());
                //handleIncompleteError(msg.newReply(true));
                return;
            }

            if (blockOption.M > 0) {
                Message reply = null;

                if (message is Response) {
                    var uri = ((Response) message).Request.Uri;
                    reply = new Request(CodeRegistry.Get, message.IsConfirmable) { Uri = uri, Token = message.Token };
                    reply.AddOption(new BlockOption(OptionNumber.Block2, blockOption.Num + 1, 0, blockOption.Szx));
                }
                else if (message is Request) {
                    // ...
                }
                else {
                    //Log.error(this, "Unsupported message type: %s", msg.key());
                    return;
                }

                SendMessageOverLowerLayer(reply);
            }
            else {
                base.OnReceive(_incomplete[key]);
                _incomplete.Remove(key);
            }
        }
    }
}