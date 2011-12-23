namespace Catarinum.Coap {
    public interface IResource {
        bool CanGet(byte[] uriPath);
        byte[] Get(byte[] uriPath);
    }
}