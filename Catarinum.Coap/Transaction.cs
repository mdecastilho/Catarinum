using System;

namespace Catarinum.Coap {
    public class Transaction : ITransaction {
        public const int ResponseTimeout = 2000;
        public const double ResponseRandomFactor = 1.5;
        public const int MaxRetransmissions = 4;
        private readonly ITimer _timer;
        public Message Message { get; set; }
        public int Retransmissions { get; set; }
        public int Timeout { get; set; }

        public Transaction(ITimer timer) {
            _timer = timer;
        }

        public void ScheduleRetransmission(Action<ITransaction> callback, Action<ITransaction> error) {
            Timeout = GetInitialTimeout();
            _timer.Start(transaction => {
                if (transaction.Retransmissions >= MaxRetransmissions) {
                    error(transaction);
                    return;
                }

                transaction.Retransmissions++;
                transaction.Timeout *= 2;
                _timer.SetTimeout(transaction.Timeout);
                callback(transaction);
            }, this, Timeout);
        }

        public void Cancel() {
            _timer.Stop();
        }

        private static int GetInitialTimeout() {
            const int min = ResponseTimeout;
            const int max = (int) (ResponseTimeout * ResponseRandomFactor);
            var random = new Random();
            return random.Next(min, max);
        }
    }
}