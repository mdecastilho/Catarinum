using System;
using System.Net;
using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly TransportLayer _transportLayer;
        private readonly MessageHandler _handler;

        public Server() {
            _transportLayer = new TransportLayer();
            _handler = new MessageHandler(_transportLayer, new TemperatureResource());
        }

        public void Start(IPEndPoint endPoint) {
            _transportLayer.Listen(endPoint);
            _transportLayer.Receive(OnReceive);
        }

        private void OnReceive(Message message) {
            Console.WriteLine(string.Format("message received: {0}", message.UriPath));
            _handler.HandleRequest((Request) message);
        }
    }
}