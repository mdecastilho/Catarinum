using System;
using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class PrintRequestHandler : IMessageHandler {
        public void Handle(Message message) {
            Console.WriteLine(string.Format("request received: {0}", message.UriPath));
        }
    }
}