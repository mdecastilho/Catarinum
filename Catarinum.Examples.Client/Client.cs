using System;
using Catarinum.Coap;
using Catarinum.Examples.Server;
using Catarinum.Util;

namespace Catarinum.Examples.Client {
    public class Client {
        public void Start() {
            var uri = new Uri("coap://127.0.0.1/temperature");
            var request = new Request(1, CodeRegistry.Get, true) { Uri = uri, Token = ByteConverter.GetBytes(0xcafe) };
            var transportLayer = new TransportLayer();
            transportLayer.Send(request);
            transportLayer.Receive(OnReceive);
        }

        private static void OnReceive(Message message) {
            Console.WriteLine(string.Format("message received: {0}", ByteConverter.GetString(message.Payload)));
        }
    }
}