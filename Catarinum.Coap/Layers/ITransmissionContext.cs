using System;

namespace Catarinum.Coap.Layers {
    public interface ITransmissionContext {
        Message Message { get; set; }
        int Timeout { get; set; }
        int Retries { get; set; }
        void ScheduleRetry(Action<ITransmissionContext> retry, Action<ITransmissionContext> error);
        void Cancel();
    }
}