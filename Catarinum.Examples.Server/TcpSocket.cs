using System;
using System.Net;
using System.Net.Sockets;

namespace Catarinum.Examples.Server {
    public class TcpSocket : ISocket {
        private readonly Socket _socket;

        public IPEndPoint LocalEndPoint {
            get { return (IPEndPoint) _socket.LocalEndPoint; }
        }

        public IPEndPoint RemoteEndPoint {
            get { return (IPEndPoint) _socket.RemoteEndPoint; }
        }

        public bool Connected {
            get { return _socket.Connected; }
        }

        public TcpSocket() {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Configure(_socket);
        }

        public TcpSocket(Socket socket) {
            _socket = socket;
        }

        public void Listen(IPEndPoint endPoint) {
            _socket.Bind(endPoint);
            _socket.Listen((int) SocketOptionName.MaxConnections);
        }

        public void BeginAccept(AsyncCallback callback, object state) {
            _socket.BeginAccept(callback, state);
        }

        public ISocket EndAccept(IAsyncResult asyncResult) {
            var incomingSocket = _socket.EndAccept(asyncResult);
            Configure(incomingSocket);
            return new TcpSocket(incomingSocket);
        }

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, AsyncCallback callback, object state) {
            return _socket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state);
        }

        public int EndReceive(IAsyncResult asyncResult) {
            return _socket.EndReceive(asyncResult);
        }

        public void Send(byte[] buffer, int size) {
            _socket.Send(buffer, size, SocketFlags.None);
        }

        public void Dispose() {
            _socket.Disconnect(false);
            _socket.Dispose();
        }

        private static void Configure(Socket socket) {
            socket.LingerState = new LingerOption(true, 0);
            socket.NoDelay = true;
        }
    }
}