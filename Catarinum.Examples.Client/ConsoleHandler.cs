using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;

namespace Catarinum.Examples.Client {
    public class ConsoleHandler : IMessageObserver {
        public void OnSend(Message message) {
            var request = message as Request;

            if (request != null) {
                Console.WriteLine(string.Format("request sent ({0}): {1}", request.Id, request.Uri));
            }
        }

        public void OnReceive(Message message) {
            var response = message as Response;

            if (response != null) {
                Console.WriteLine(string.Format("response received ({0}): {1}", message.Id, ByteConverter.GetString(message.Payload)));
            }
        }

        public void OnError(Exception e) {
            Console.WriteLine(string.Format("[ERROR] {0}", e.Message));
        }
    }
}