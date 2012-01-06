using System;

namespace Catarinum.Coap.Impl {
    public class Transaction : ITransaction {
        public const int ResponseTimeout = 2000;
        public const double ResponseRandomFactor = 1.5;
        public const int MaxRetransmissions = 4;
        private readonly MessageLayer _messageLayer;
        private readonly ITimer _timer;
        public Message Message { get; set; }
        public int Retransmissions { get; set; }
        public int Timeout { get; set; }

        public Transaction(MessageLayer messageLayer, ITimer timer) {
            _messageLayer = messageLayer;
            _timer = timer;
        }

        public void ScheduleRetransmission() {
            const int min = ResponseTimeout;
            const int max = (int) (ResponseTimeout * ResponseRandomFactor);
            var random = new Random();
            Timeout = random.Next(min, max);
            _timer.Start(HandleResponseTimeout, this, Timeout);
        }

        public void Cancel() {
            _timer.Stop();
        }

        private void HandleResponseTimeout(object state) {
            var transaction = (Transaction) state;

            if (transaction.Retransmissions >= MaxRetransmissions) {
                _timer.Stop();
                _messageLayer.RemoveTransaction(transaction.Message);
                return;
            }

            transaction.Retransmissions++;
            transaction.Timeout *= 2;
            _timer.SetTimeout(transaction.Timeout);
            _messageLayer.SendMessageOverLowerLayer(transaction.Message);
        }
    }
}