using Catarinum.Coap.Layers;

namespace Catarinum.Coap {
    public interface ITransmissionContextFactory {
        ITransmissionContext Create(MessageLayer messageLayer, Message message);
    }
}