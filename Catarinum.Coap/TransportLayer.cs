using System;
using System.Net;
using System.Net.Sockets;

namespace Catarinum.Coap {
    public class TransportLayer : Layer {
        private readonly MessageSerializer _messageSerializer;
        private readonly Socket _socket;
        private readonly byte[] _buffer;
        private bool _isListening;

        public TransportLayer() {
            _messageSerializer = new MessageSerializer();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _buffer = new byte[1024];
        }

        public void Listen(string ipAddress, int port) {
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _socket.Bind(endPoint);
            _isListening = true;
            BeginReceive(new IPEndPoint(IPAddress.Any, 0));
        }

        public override void Send(Message message) {
            var endPoint = new IPEndPoint(IPAddress.Parse(message.RemoteAddress), message.Port);

            try {
                var bytes = _messageSerializer.Serialize(message);
                _socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, endPoint, OnSend, null);

                if (!_isListening) {
                    BeginReceive(endPoint);
                }
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("send message error: {0}", e.Message));
            }
        }

        private void BeginReceive(EndPoint sender) {
            try {
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
                var bytesRead = _socket.EndReceiveFrom(ar, ref sender);

                if (bytesRead > 0) {
                    var bytes = new byte[bytesRead];
                    Buffer.BlockCopy(_buffer, 0, bytes, 0, bytesRead);
                    var message = _messageSerializer.Deserialize(bytes);
                    message.RemoteAddress = ((IPEndPoint) sender).Address.ToString();
                    message.Port = ((IPEndPoint) sender).Port;
                    Handle(message);
                    _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref sender, OnReceive, null);
                }
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("receive message error: {0}", e.Message));
            }
        }
    }
}