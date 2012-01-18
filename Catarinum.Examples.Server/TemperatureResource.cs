using System;
using System.Threading;
using Catarinum.Coap;
using Catarinum.Coap.Layers;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Server {
    public class TemperatureResource : IResource {
        public bool CanGet(Uri uri) {
            return true;
        }

        public byte[] Get(Uri uri) {
            var random = new Random();
            var t = random.Next(MessageLayer.ResponseTimeout, (int) (MessageLayer.ResponseTimeout * MessageLayer.ResponseRandomFactor));
            Thread.Sleep(t);
            return ByteConverter.GetBytes("22.3 C");
        }
    }
}