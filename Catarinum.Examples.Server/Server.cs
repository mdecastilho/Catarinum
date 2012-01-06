using Catarinum.Coap.Impl;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly TransportLayer _transportLayer;
        private readonly MessageLayer _messageLayer;

        public Server() {
            _transportLayer = new TransportLayer();
            _messageLayer = new MessageLayer(_transportLayer);
            _messageLayer.AddHandler(new ConsoleHandler());
            _messageLayer.AddHandler(new ResourceHandler(_messageLayer, new TemperatureResource()));
        }

        public void Start(string ipAddress, int port) {
            _transportLayer.Listen(ipAddress, port);
        }
    }
}