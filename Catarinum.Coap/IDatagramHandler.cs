namespace Catarinum.Coap {
    public interface IDatagramHandler {
        void Handle(string ipAddress, int port, byte[] bytes);
    }
}