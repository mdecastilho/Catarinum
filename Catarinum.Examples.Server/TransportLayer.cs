using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Catarinum.Coap;

namespace Catarinum.Examples.Server {
    public class TransportLayer : ITransportLayer {
        private readonly List<IDatagramHandler> _handlers;
        private readonly Socket _socket;
        private readonly byte[] _buffer;

        public TransportLayer() {
            _handlers = new List<IDatagramHandler>();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _buffer = new byte[1024];
        }

        public void Listen(string ipAddress, int port) {
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _socket.Bind(endPoint);
            BeginReceive();
        }

        public void Send(string ipAddress, int port, byte[] bytes) {
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            try {
                _socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, endPoint, OnSend, null);
                BeginReceive();
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("send message error: {0}", e.Message));
            }
        }

        public void AddHandler(IDatagramHandler handler) {
            _handlers.Add(handler);
        }

        private void BeginReceive() {
            try {
                var sender = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref sender, OnReceive, null);
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("receive message error: {0}", e.Message));
            }
        }

        private void OnSend(IAsyncResult ar) {
            try {
                _socket.EndSend(ar);
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("send message error: {0}", e.Message));
            }
        }

        private void OnReceive(IAsyncResult ar) {
            try {
                EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                _socket.EndReceiveFrom(ar, ref sender);
                var ipAddress = ((IPEndPoint) sender).Address.ToString();
                var port = ((IPEndPoint) sender).Port;

                foreach (var handler in _handlers) {
                    handler.Handle(ipAddress, port, _buffer);
                }

                _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref sender, OnReceive, null);
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("receive message error: {0}", e.Message));
            }
        }
    }
}