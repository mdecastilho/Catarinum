namespace Catarinum {
    public interface IResource {
        bool IsContextMissing(byte[] uri);
        bool CanGet(byte[] uri);
        byte[] Get(byte[] uri);
    }
}