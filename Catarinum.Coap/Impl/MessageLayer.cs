namespace Catarinum.Coap.Impl {
    public class MessageLayer : IMessageLayer {
        private readonly ITransportLayer _transportLayer;
        private readonly IMessageSerializer _messageSerializer;
        private readonly MessageReceiver _messageReceiver;

        public MessageLayer()
            : this(new TransportLayer(), new MessageSerializer()) {
        }

        public MessageLayer(ITransportLayer transportLayer, IMessageSerializer messageSerializer) {
            _transportLayer = transportLayer;
            _messageSerializer = messageSerializer;
            _messageReceiver = new MessageReceiver(messageSerializer);
            _transportLayer.AddHandler(_messageReceiver);
        }

        public void Listen(string ipAddress, int port) {
            _transportLayer.Listen(ipAddress, port);
        }

        public void Send(Message message) {
            var bytes = _messageSerializer.Serialize(message);
            _transportLayer.Send(message.RemoteAddress, message.Port, bytes);
        }

        public void AddHandler(IMessageHandler handler) {
            _messageReceiver.AddMessageHandler(handler);
        }
    }
}