using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly MessageLayer _messageLayer;

        public Server() {
            _messageLayer = new MessageLayer(new TransportLayer());
            _messageLayer.AddHandler(new PrintRequestHandler());
            _messageLayer.AddHandler(new MessageHandler(_messageLayer, new TemperatureResource()));
        }

        public void Start(string ipAddress, int port) {
            _messageLayer.Listen(ipAddress, port);
        }
    }
}