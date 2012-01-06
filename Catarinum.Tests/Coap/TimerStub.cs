using System;
using System.Collections.Generic;
using System.Linq;
using Catarinum.Coap;

namespace Catarinum.Tests.Coap {
    public class TimerStub : ITimer {
        public List<int> Timeouts { get; set; }
        public int Ticks { get; set; }
        public int Count { get; set; }

        public int Timeout {
            get { return Timeouts.Last(); }
            set { Timeouts.Add(value); }
        }

        public void Start(Action<ITransaction> callback, ITransaction state, int timeout) {
            Timeouts = new List<int> { timeout };

            while (Count < Ticks) {
                callback(state);
                Count++;
            }
        }

        public void Stop() { }
    }
}