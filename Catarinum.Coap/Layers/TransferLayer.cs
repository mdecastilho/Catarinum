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
            var blockInfo = NegotiateBlockSize(message);
            var isBlockwise = message.Payload.Length > BlockOption.DecodeSzx(blockInfo.Szx);

            if (isBlockwise) {
                if (!message.HasToken) {
                    message.Token = _tokenManager.AcquireToken();
                }

                var block = message.GetBlock(blockInfo);
                var optionNumber = message is Request ? OptionNumber.Block1 : OptionNumber.Block2;

                if (block.GetBlockOption(optionNumber).M > 0) {
                    _incomplete.Add(message.GetTransactionKey(), message);
                }

                SendMessageOverLowerLayer(block);
            }
            else {
                SendMessageOverLowerLayer(message);
            }
        }

        public override void OnReceive(Message message) {
            if (!message.IsBlockwise()) {
                base.OnReceive(message);
            }
            else if (message.IsBlockwiseRequest()) {
                SendBlockwiseResponse(message);
            }
            else if (message.IsBlockwiseAcknowledgement()) {
                HandleBlockwiseAcknowledgement(message);
            }
            else if (message.IsBlockwiseResponse()) {
                HandleIncomingPayload(message);
            }
        }

        private BlockInfo NegotiateBlockSize(Message message) {
            var blockInfo = new BlockInfo { Szx = _szx };
            var response = message as Response;

            if (response != null) {
                var block2 = response.Request.GetBlockOption(OptionNumber.Block2);

                if (block2 != null) {
                    blockInfo.Num = block2.Num;

                    if (block2.Szx < _szx) {
                        blockInfo.Szx = block2.Szx;
                    }
                }
            }

            return blockInfo;
        }

        private void SendBlockwiseResponse(Message message) {
            var key = message.GetTransactionKey();
            var block2 = message.GetBlockOption(OptionNumber.Block2);

            if (!_incomplete.ContainsKey(key)) {
                base.OnReceive(message);
            }
            else {
                var first = _incomplete[key];
                var block = first.GetBlock(block2.Num, block2.Szx);

                if (block != null) {
                    block.Id = message.Id;
                    var m = block.GetBlockOption(OptionNumber.Block2).M;
                    SendMessageOverLowerLayer(block);

                    if (m == 0) {
                        _incomplete.Remove(key);
                    }
                }
            }
        }

        private void HandleBlockwiseAcknowledgement(Message message) {
            var key = message.GetTransactionKey();
            var first = _incomplete[key];
            var block1 = message.GetBlockOption(OptionNumber.Block1);

            if (block1.M > 0) {
                var block = first.GetBlock(block1.Num + 1, block1.Szx);
                SendMessageOverLowerLayer(block);
            }
            else {
                _incomplete.Remove(key);
            }
        }

        private void HandleIncomingPayload(Message message) {
            var key = message.GetTransactionKey();
            var block2 = message.GetBlockOption(OptionNumber.Block2);

            if (_incomplete.ContainsKey(key)) {
                _incomplete[key].Id = message.Id;
                _incomplete[key].AppendPayload(message.Payload);
                _incomplete[key].SetOption(block2);
            }
            else {
                _incomplete.Add(key, message);
            }

            if (block2.M > 0) {
                var request = ((Response) message).Request;
                var reply = new Request(CodeRegistry.Get, message.IsConfirmable) { Uri = request.Uri, Token = message.Token };
                reply.AddOption(new BlockOption(OptionNumber.Block2, block2.Num + 1, 0, block2.Szx));
                SendMessageOverLowerLayer(reply);
            }
            else {
                base.OnReceive(_incomplete[key]);
                _incomplete.Remove(key);
            }
        }
    }
}