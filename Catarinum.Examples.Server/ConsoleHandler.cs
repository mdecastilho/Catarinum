using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Server {
    public class ConsoleHandler : IHandler {
        public void Handle(Message message) {
            var response = message as Response;

            if (response != null) {
                Console.WriteLine(string.Format("response ({0}): {1}", message.Id, ByteConverter.GetString(message.Payload)));
            }
        }
    }
}