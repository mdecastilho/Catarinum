namespace Catarinum.Coap {
    public interface IHandler {
        void Handle(Message message);
    }
}