using System;

namespace Catarinum.Coap {
    public class IdSequence {
        public const int MaxId = (1 << Message.IdBits - 1);
        public int CurrentId { get; set; }

        public IdSequence() {
            var rand = new Random();
            CurrentId = rand.Next(1, MaxId);
        }

        public int NextId() {
            lock (this) {
                CurrentId++;

                if (CurrentId > MaxId) {
                    CurrentId = 1;
                }

                return CurrentId;
            }
        }
    }
}