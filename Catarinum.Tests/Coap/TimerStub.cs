using System;
using System.Collections.Generic;
using Catarinum.Coap;

namespace Catarinum.Tests.Coap {
    public class TimerStub : ITimer {
        public List<int> Timeouts { get; set; }
        public int Ticks { get; set; }
        public int Count { get; set; }

        public void Start(Action<ITransmissionContext> callback, ITransmissionContext state, int timeout) {
            Timeouts = new List<int> { timeout };

            while (Count < Ticks) {
                callback(state);
                Count++;
            }
        }

        public void Stop() { }

        public void SetTimeout(int timeout) {
            Timeouts.Add(timeout);
        }
    }
}