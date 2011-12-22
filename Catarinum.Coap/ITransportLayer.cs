namespace Catarinum.Coap {
    public interface ITransportLayer {
        void Send(Message message);
    }
}