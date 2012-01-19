using System;

namespace Catarinum.Coap.Layers {
    public interface ITransmissionTimer {
        void Start(Action<ITransmissionContext> callback, ITransmissionContext state, int timeout);
        void Stop();
        void SetTimeout(int timeout);
    }
}