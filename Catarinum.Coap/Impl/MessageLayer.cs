using System.Collections.Generic;

namespace Catarinum.Coap.Impl {
    public class MessageLayer : UpperLayer {
        private readonly ITransactionFactory _transactionFactory;
        private readonly Dictionary<int, ITransaction> _transactions;

        public MessageLayer(ILayer lowerLayer)
            : this(lowerLayer, new TransactionFactory()) {
        }

        public MessageLayer(ILayer lowerLayer, ITransactionFactory transactionFactory)
            : base(lowerLayer) {
            _transactionFactory = transactionFactory;
            _transactions = new Dictionary<int, ITransaction>();
        }

        public IEnumerable<ITransaction> Transactions {
            get { return _transactions.Values; }
        }

        public override void Send(Message message) {
            if (message.Id == 0) {
                message.Id = IdGenerator.NextId();
            }

            if (message.IsConfirmable) {
                var transaction = _transactionFactory.Create(this, message);
                _transactions.Add(message.Id, transaction);
                transaction.ScheduleRetransmission();
            }

            SendMessageOverLowerLayer(message);
        }

        public override void Handle(Message message) {
            if (message.IsReply) {
                if (_transactions.ContainsKey(message.Id)) {
                    var transaction = _transactions[message.Id];
                    transaction.Cancel();
                    _transactions.Remove(message.Id);
                }
            }

            base.Handle(message);
        }

        public void RemoveTransaction(Message message) {
            _transactions.Remove(message.Id);
        }
    }
}