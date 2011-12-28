namespace Catarinum.Coap {
    public interface ITransportLayer {
        void Listen(string ipAddress, int port);
        void Send(string ipAddress, int port, byte[] bytes);
        void AddHandler(IDatagramHandler handler);
    }
}