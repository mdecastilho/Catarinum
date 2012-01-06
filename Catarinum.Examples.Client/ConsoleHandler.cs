using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Client {
    public class ConsoleHandler : IHandler {
        public void Handle(Message message) {
            var response = message as Response;

            if (response != null) {
                Console.WriteLine(string.Format("response received ({0}): {1}", message.Id, ByteConverter.GetString(message.Payload)));
            }
        }
    }
}