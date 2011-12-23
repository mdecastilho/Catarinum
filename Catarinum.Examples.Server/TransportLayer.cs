using System;
using System.Net;
using System.Net.Sockets;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;

namespace Catarinum.Examples.Server {
    public class TransportLayer : ITransportLayer {
        private readonly Socket _socket;
        private byte[] _buffer;

        public TransportLayer() {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Listen(EndPoint endPoint) {
            Console.WriteLine(string.Format("listening at {0}...", endPoint));
            _socket.Bind(endPoint);
        }

        public void Send(Message message) {
            var endPoint = new IPEndPoint(IPAddress.Parse(message.RemoteAddress), message.Port);

            try {
                var bytes = MessageSerializer.Serialize(message);
                _socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, endPoint, OnSend, null);
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("send message error: {0}", e.Message));
            }
        }

        public void Receive(Action<Message> callback) {
            try {
                var sender = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _buffer = new byte[1024];
                _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref sender, OnReceive, callback);
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
                var callback = (Action<Message>) ar.AsyncState;
                var message = MessageSerializer.Unserialize(_buffer);
                message.RemoteAddress = ((IPEndPoint) sender).Address.ToString();
                message.Port = ((IPEndPoint) sender).Port;
                callback(message);
                _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref sender, OnReceive, null);
            }
            catch (Exception e) {
                Console.WriteLine(string.Format("receive message error: {0}", e.Message));
            }
        }
    }
}