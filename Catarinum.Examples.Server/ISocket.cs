using System;
using System.Net;

namespace Catarinum.Examples.Server {
    public interface ISocket {
        IPEndPoint LocalEndPoint { get; }
        IPEndPoint RemoteEndPoint { get; }
        bool Connected { get; }
        void Listen(IPEndPoint endPoint);
        void BeginAccept(AsyncCallback callback, object state);
        ISocket EndAccept(IAsyncResult asyncResult);
        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        int EndReceive(IAsyncResult asyncResult);
        void Send(byte[] buffer, int size);
        void Dispose();
    }
}