using System;
using Catarinum.Coap;
using Catarinum.Coap.Impl;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Client {
    public class Client {
        private readonly MessageLayer _messageLayer;

        public Client() {
            _messageLayer = new MessageLayer(new TransportLayer());
            _messageLayer.AddHandler(new ConsoleHandler());
        }

        public void SendRequest() {
            var uri = new Uri("coap://127.0.0.1/temperature");
            var request = new Request(1, CodeRegistry.Get, true) { Uri = uri, Token = ByteConverter.GetBytes(0xcafe) };
            _messageLayer.Send(request);
        }
    }
}