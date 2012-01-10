namespace Catarinum.Coap {
    public interface ILayer {
        void Send(Message message);
        void RegisterObserver(IMessageObserver observer);
    }
}