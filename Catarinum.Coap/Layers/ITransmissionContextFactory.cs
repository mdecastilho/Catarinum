namespace Catarinum.Coap.Layers {
    public interface ITransmissionContextFactory {
        ITransmissionContext Create(MessageLayer messageLayer, Message message);
    }
}