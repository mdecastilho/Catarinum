using System;

namespace Catarinum.Coap {
    public interface ITimer {
        void Start(Action<ITransmissionContext> callback, ITransmissionContext state, int timeout);
        void Stop();
        void SetTimeout(int timeout);
    }
}