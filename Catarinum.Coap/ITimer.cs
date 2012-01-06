using System;

namespace Catarinum.Coap {
    public interface ITimer {
        int Timeout { get; set; }
        void Start(Action<ITransaction> callback, ITransaction state, int timeout);
        void Stop();
    }
}