using Catarinum.Coap;
using Catarinum.Util;

namespace Catarinum.Examples.Server {
    public class TemperatureResource : IResource {
        public bool CanGet(byte[] uriPath) {
            return true;
        }

        public byte[] Get(byte[] uriPath) {
            return ByteConverter.GetBytes("22.3 C");
        }
    }
}