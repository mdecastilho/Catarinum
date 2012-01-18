using System;

namespace Catarinum.Coap {
    public class TransmissionContext : ITransmissionContext {
        private readonly ITimer _timer;
        public Message Message { get; set; }
        public int Timeout { get; set; }
        public int Retries { get; set; }

        public TransmissionContext(ITimer timer) {
            _timer = timer;
        }

        public void ScheduleRetry(Action<ITransmissionContext> retry, Action<ITransmissionContext> error) {
            Timeout = GetInitialTimeout();
            _timer.Start(ctx => {
                if (ctx.Retries >= MessageLayer.MaxRetransmissions) {
                    error(ctx);
                    return;
                }

                ctx.Retries++;
                ctx.Timeout *= 2;
                _timer.SetTimeout(ctx.Timeout);
                retry(ctx);
            }, this, Timeout);
        }

        public void Cancel() {
            _timer.Stop();
        }

        private static int GetInitialTimeout() {
            const int min = MessageLayer.ResponseTimeout;
            const int max = (int) (MessageLayer.ResponseTimeout * MessageLayer.ResponseRandomFactor);
            var random = new Random();
            return random.Next(min, max);
        }
    }
}