using System;
using Catarinum.Coap;
using Catarinum.Coap.Impl;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Client {
    public class Client {
        public void Start() {
            var uri = new Uri("coap://127.0.0.1/temperature");
            var request = new Request(1, CodeRegistry.Get, true) { Uri = uri, Token = ByteConverter.GetBytes(0xcafe) };
            var messageLayer = new MessageLayer();
            messageLayer.AddHandler(new PrintResponseHandler());
            messageLayer.Send(request);
        }
    }
}