namespace Catarinum.Coap {
    public interface IResource {
        bool CanGet(byte[] uri);
        byte[] Get(byte[] uri);
    }
}