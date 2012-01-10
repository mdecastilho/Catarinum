using System;
using System.Threading;

namespace Catarinum.Coap {
    public class RetransmissionTimer : ITimer {
        private Timer _timer;
        private Action<ITransaction> _callback;

        public void Start(Action<ITransaction> callback, ITransaction state, int timeout) {
            _callback = callback;
            _timer = new Timer(TimerCallback, state, timeout, timeout);
        }

        public void Stop() {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer = null;
        }

        public void SetTimeout(int timeout) {
            if (_timer != null) {
                _timer.Change(timeout, timeout);
            }
        }

        private void TimerCallback(object state) {
            _callback((Transaction) state);
        }
    }
}