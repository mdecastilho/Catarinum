using System;
using System.Net;
using System.Net.Sockets;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;

namespace Catarinum.Examples.Client {
    class Program {
        static void Main(string[] args) {
            var uri = new Uri("coap://127.0.0.1/temperature");
            var request = new Request(1, CodeRegistry.Get, true);
            request.AddUri(uri);
            request.AddToken(ByteConverter.GetBytes(0xcafe));
            var remoteAddress = request.RemoteAddress.Split(':');
            var endPoint = new IPEndPoint(IPAddress.Parse(remoteAddress[0]), Convert.ToInt32(remoteAddress[1]));
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                socket.Connect(endPoint);
                Console.WriteLine(string.Format("Conectou em {0}", endPoint));
                socket.Send(MessageSerializer.Serialize(request));
                Console.WriteLine("Enviou mensagem!");
                socket.Close();
            }
            catch (Exception e) {
                socket.Close();
                Console.WriteLine(string.Format("Erro ao conectar em {0}: {1}", endPoint, e.Message));
            }

            Console.ReadLine();
        }
    }
}
