using System;
using System.Threading;

namespace Catarinum.Coap.Impl {
    public class RetransmissionTimer : ITimer {
        private Timer _timer;
        private Action<ITransaction> _callback;
        private int _timeout;

        public int Timeout {
            get { return _timeout; }
            set {
                _timeout = value;
                _timer.Change(_timeout, _timeout);
            }
        }

        public void Start(Action<ITransaction> callback, ITransaction state, int timeout) {
            _callback = callback;
            _timeout = timeout;
            _timer = new Timer(TimerCallback, state, timeout, timeout);
        }

        public void Stop() {
            _timer.Dispose();
        }

        private void TimerCallback(object state) {
            _callback((Transaction) state);
        }
    }
}