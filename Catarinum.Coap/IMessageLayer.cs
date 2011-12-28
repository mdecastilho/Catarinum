namespace Catarinum.Coap {
    public interface IMessageLayer {
        void Listen(string ipAddress, int port);
        void Send(Message message);
        void AddHandler(IMessageHandler handler);
    }
}