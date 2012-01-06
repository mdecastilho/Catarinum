using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly MessageLayer _messageLayer;

        public Server() {
            _messageLayer = new MessageLayer();
            _messageLayer.AddHandler(new ConsoleHandler());
            _messageLayer.AddHandler(new ResourceHandler(_messageLayer, new TemperatureResource()));
        }

        public void Start(string ipAddress, int port) {
            ((TransportLayer) _messageLayer.LowerLayer).Listen(ipAddress, port);
        }
    }
}