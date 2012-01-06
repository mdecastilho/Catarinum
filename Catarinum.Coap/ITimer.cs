using System;

namespace Catarinum.Coap {
    public interface ITimer {
        void Start(Action<ITransaction> callback, ITransaction state, int timeout);
        void Stop();
        void SetTimeout(int timeout);
    }
}