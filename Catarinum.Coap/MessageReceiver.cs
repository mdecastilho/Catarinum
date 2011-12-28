using System.Collections.Generic;

namespace Catarinum.Coap {
    public class MessageReceiver : IDatagramHandler {
        private readonly List<IMessageHandler> _handlers;

        public MessageReceiver() {
            _handlers = new List<IMessageHandler>();
        }

        public void Handle(string ipAddress, int port, byte[] bytes) {
            try {
                var message = MessageSerializer.Deserialize(bytes);
                message.RemoteAddress = ipAddress;
                message.Port = port;

                foreach (var handler in _handlers) {
                    handler.Handle(message);
                }
            }
            catch {
                // unable to deserialize message
            }
        }

        public void AddMessageHandler(IMessageHandler handler) {
            _handlers.Add(handler);
        }
    }
}