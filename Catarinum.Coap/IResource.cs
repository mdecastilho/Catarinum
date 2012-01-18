using System;

namespace Catarinum.Coap {
    public interface IResource {
        bool CanGet(Uri uri);
        byte[] Get(Uri uri);
    }
}