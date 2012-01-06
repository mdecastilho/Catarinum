using System;

namespace Catarinum.Coap {
    public static class IdGenerator {
        public const int MaxId = (1 << Message.IdBits - 1);
        private static int _id;

        static IdGenerator() {
            var rand = new Random();
            _id = rand.Next(1, MaxId);
        }

        public static int NextId() {
            var id = _id;
            _id++;

            if (_id > MaxId) {
                _id = 1;
            }

            return id;
        }
    }
}