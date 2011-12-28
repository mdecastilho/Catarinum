using System;

namespace Catarinum.Examples.Server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("***SERVER***");
            var server = new Server();
            server.Start("127.0.0.1", 5683);
            Console.ReadLine();
        }
    }
}
