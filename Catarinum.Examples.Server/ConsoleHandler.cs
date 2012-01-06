using System;
using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class ConsoleHandler : IHandler {
        public void Handle(Message message) {
            var request = message as Request;

            if (request != null) {
                Console.WriteLine(string.Format("request received ({0}): {1}", request.Id, request.UriPath));
            }
        }
    }
}