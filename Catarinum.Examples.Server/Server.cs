using Catarinum.Coap.Impl;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly MessageLayer _messageLayer;

        public Server() {
            _messageLayer = new MessageLayer();
            _messageLayer.AddHandler(new PrintRequestHandler());
            _messageLayer.AddHandler(new RequestHandler(_messageLayer, new TemperatureResource()));
        }

        public void Start(string ipAddress, int port) {
            _messageLayer.Listen(ipAddress, port);
        }
    }
}