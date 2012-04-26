using System;

namespace Catarinum.Coap.Layers {
    public class TransportLayerException : Exception {
        public TransportLayerException(string message)
            : base(message) {
        }
    }
}