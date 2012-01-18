using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public class TokenManager {
        public static readonly byte[] EmptyToken = new byte[0];

        private int _nextValue;

        public TokenManager() {
            _nextValue = 0;
        }

        public byte[] AcquireToken() {
            var token = ByteConverter.GetBytes(_nextValue++);
            return token;
        }
    }
}