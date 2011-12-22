using System;
using System.Collections.Generic;
using System.Net;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;

namespace Catarinum.Examples.Server {
    public class Server {
        private readonly ISocket _listener;
        private readonly List<ConnectionInfo> _connections;
        private readonly TransportLayer _transportLayer;
        private readonly MessageHandler _handler;

        public Server() {
            _listener = new TcpSocket();
            _transportLayer = new TransportLayer();
            _handler = new MessageHandler(_transportLayer, new TemperatureResource());
            _connections = new List<ConnectionInfo>();
        }

        public void Start(IPEndPoint endPoint) {
            Console.WriteLine(string.Format("aguardando conexoes em {0}...", endPoint));
            _listener.Listen(endPoint);

            for (var i = 0; i < 1000; i++) {
                _listener.BeginAccept(AcceptCallback, _listener);
            }
        }

        private void AcceptCallback(IAsyncResult result) {
            var connection = new ConnectionInfo { Buffer = new byte[255] };

            try {
                var socket = (ISocket) result.AsyncState;
                connection.Socket = socket.EndAccept(result);
                Console.WriteLine("conexao recebida em {0}", _listener.LocalEndPoint);

                lock (_connections) {
                    _connections.Add(connection);
                }

                connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, ReceiveCallback, connection);
                _listener.BeginAccept(AcceptCallback, result.AsyncState);
            }
            catch (Exception e) {
                Console.WriteLine("erro em {0}: {1}", _listener.LocalEndPoint, e.Message);
                Dispose(connection);
            }
        }

        private void ReceiveCallback(IAsyncResult result) {
            var connection = (ConnectionInfo) result.AsyncState;

            try {
                var bytesRead = connection.Socket.EndReceive(result);

                if (bytesRead > 0) {
                    var bytes = new byte[bytesRead];
                    Buffer.BlockCopy(connection.Buffer, 0, bytes, 0, bytesRead);
                    _transportLayer.Socket = connection.Socket;
                    _handler.HandleRequest((Request) MessageSerializer.Unserialize(bytes));
                    connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, ReceiveCallback, connection);
                }
                else {
                    Console.WriteLine("conexao encerrada em {0}", _listener.LocalEndPoint);
                    Dispose(connection);
                }
            }
            catch (Exception e) {
                Console.WriteLine("erro em {0}: {1}", _listener.LocalEndPoint, e.Message);
                Dispose(connection);
            }
        }

        private void Dispose(ConnectionInfo connection) {
            if (connection.Socket != null) {
                connection.Socket.Dispose();
            }

            lock (_connections) {
                _connections.Remove(connection);
            }
        }
    }
}