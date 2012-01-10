using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Server {
    public class ConsoleHandler : IMessageObserver {
        public void OnSend(Message message) {
            var response = message as Response;

            if (response != null) {
                Console.WriteLine(string.Format("response sent ({0}): {1}", message.Id, ByteConverter.GetString(message.Payload)));
            }
        }

        public void OnReceive(Message message) {
            var request = message as Request;

            if (request != null) {
                Console.WriteLine(string.Format("request received ({0}): {1}", request.Id, request.UriPath));
            }
        }
    }
}