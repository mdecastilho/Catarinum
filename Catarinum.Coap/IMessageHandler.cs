namespace Catarinum.Coap {
    public interface IMessageHandler {
        void Handle(Message message);
    }
}