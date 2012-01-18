namespace Catarinum.Coap {
    public class TransmissionContextFactory : ITransmissionContextFactory {
        public ITransmissionContext Create(MessageLayer messageLayer, Message message) {
            return new TransmissionContext(new RetransmissionTimer()) { Message = message };
        }
    }
}