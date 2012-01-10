using System.Collections.Generic;

namespace Catarinum.Coap {
    public class MessageLayer : UpperLayer {
        private readonly ITransactionFactory _transactionFactory;
        private readonly Dictionary<int, ITransaction> _transactions;
        private readonly IdSequence _idSequence;
        private readonly MessageCache _replyCache;
        private readonly MessageCache _duplicationCache;

        public MessageLayer()
            : this(new TransportLayer()) {
        }

        public MessageLayer(ILayer lowerLayer)
            : this(lowerLayer, new TransactionFactory()) {
        }

        public MessageLayer(ILayer lowerLayer, ITransactionFactory transactionFactory)
            : base(lowerLayer) {
            _transactionFactory = transactionFactory;
            _transactions = new Dictionary<int, ITransaction>();
            _idSequence = new IdSequence();
            _replyCache = new MessageCache();
            _duplicationCache = new MessageCache();
        }

        public IEnumerable<ITransaction> Transactions {
            get { return _transactions.Values; }
        }

        public override void Send(Message message) {
            if (message.Id == 0) {
                message.Id = _idSequence.NextId();
            }

            if (message.IsConfirmable) {
                var transaction = _transactionFactory.Create(this, message);
                _transactions.Add(message.Id, transaction);
                transaction.ScheduleRetransmission(RetransmissionCallback, ErrorCallback);
            }
            else if (message.IsReply) {
                _replyCache.Add(message);
            }

            SendMessageOverLowerLayer(message);
        }

        public override void OnReceive(Message message) {
            lock (_duplicationCache) {
                if (_duplicationCache.ContainsMessage(message)) {
                    if (message.IsConfirmable) {
                        var reply = _replyCache.Get(message);
                        SendMessageOverLowerLayer(reply);
                    }

                    return;
                }

                _duplicationCache.Add(message);
            }

            if (message.IsReply) {
                if (_transactions.ContainsKey(message.Id)) {
                    var transaction = _transactions[message.Id];
                    transaction.Cancel();
                    _transactions.Remove(message.Id);
                }
            }

            base.OnReceive(message);
        }

        private void RetransmissionCallback(ITransaction transaction) {
            SendMessageOverLowerLayer(transaction.Message);
        }

        private void ErrorCallback(ITransaction transaction) {
            transaction.Cancel();
            _transactions.Remove(transaction.Message.Id);
        }
    }
}