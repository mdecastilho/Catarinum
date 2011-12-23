using System;

namespace Catarinum.Examples.Client {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("***CLIENT***");
            var client = new Client();
            client.Start();
            Console.ReadLine();
        }
    }
}
