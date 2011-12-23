using System;
using System.Net;

namespace Catarinum.Examples.Server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("***SERVER***");
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5683);
            var server = new Server();
            server.Start(endPoint);
            Console.ReadLine();
        }
    }
}
