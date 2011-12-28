namespace Catarinum.Coap {
    public class MessageLayer : IMessageLayer {
        private readonly ITransportLayer _transportLayer;
        private readonly MessageReceiver _receiver;

        public MessageLayer(ITransportLayer transportLayer) {
            _receiver = new MessageReceiver();
            _transportLayer = transportLayer;
            _transportLayer.AddHandler(_receiver);
        }

        public void Listen(string ipAddress, int port) {
            _transportLayer.Listen(ipAddress, port);
        }

        public void Send(Message message) {
            var bytes = MessageSerializer.Serialize(message);
            _transportLayer.Send(message.RemoteAddress, message.Port, bytes);
        }

        public void AddHandler(IMessageHandler handler) {
            _receiver.AddMessageHandler(handler);
        }
    }
}