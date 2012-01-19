using System;

namespace Catarinum.Coap.Layers {
    public class TransmissionContext : ITransmissionContext {
        private readonly ITransmissionTimer _timer;
        public Message Message { get; set; }
        public int Timeout { get; set; }
        public int Retries { get; set; }

        public TransmissionContext(ITransmissionTimer timer) {
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
            var min = MessageLayer.ResponseTimeout;
            var max = (int) (MessageLayer.ResponseTimeout * MessageLayer.ResponseRandomFactor);
            var random = new Random();
            return random.Next(min, max);
        }
    }
}