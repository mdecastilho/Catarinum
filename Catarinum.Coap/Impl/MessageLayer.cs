namespace Catarinum.Coap.Impl {
    public class MessageLayer : UpperLayer {
        public MessageLayer(ILayer lowerLayer)
            : base(lowerLayer) {
        }

        public override void Send(Message message) {
            SendMessageOverLowerLayer(message);
        }

        public override void Handle(Message message) {
            Deliver(message);
        }
    }
}