using Catarinum.Coap;
using Catarinum.Coap.Helpers;

namespace Catarinum.Examples.Server {
    public class TemperatureResource : IResource {
        public bool CanGet(byte[] uri) {
            return true;
        }

        public byte[] Get(byte[] uri) {
            return ByteConverter.GetBytes("22.3 C");
        }
    }
}