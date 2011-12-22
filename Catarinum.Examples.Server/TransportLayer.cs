using Catarinum.Coap;
using Catarinum.Coap.Helpers;

namespace Catarinum.Examples.Server {
    public class TransportLayer : ITransportLayer {
        public ISocket Socket { get; set; }

        public void Send(Message message) {
            var bytes = MessageConverter.GetBytes(message);
            Socket.Send(bytes, bytes.Length);
        }
    }
}