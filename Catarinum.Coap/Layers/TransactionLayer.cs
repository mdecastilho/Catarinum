using System.Collections.Generic;

namespace Catarinum.Coap.Layers {
    public class TransactionLayer : UpperLayer {
        private readonly Dictionary<string, Request> _transactions;

        public TransactionLayer(ILayer lowerLayer)
            : base(lowerLayer) {
            _transactions = new Dictionary<string, Request>();
        }

        public override void Send(Message message) {
            if (message is Request) {
                _transactions[message.GetTransactionKey()] = (Request) message;
            }
        }

        public override void OnReceive(Message message) {
            if (message is Response) {
                ((Response) message).Request = _transactions[message.GetTransactionKey()];
            }

            base.OnReceive(message);
        }
    }
}