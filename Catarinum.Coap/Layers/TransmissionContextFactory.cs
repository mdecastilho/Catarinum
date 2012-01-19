namespace Catarinum.Coap.Layers {
    public class TransmissionContextFactory : ITransmissionContextFactory {
        public ITransmissionContext Create(MessageLayer messageLayer, Message message) {
            return new TransmissionContext(new TransmissionTimer()) { Message = message };
        }
    }
}