using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catarinum.Examples.Client {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("***CLIENT***");
            var client = new Client();

            Task.Factory.StartNew(() => {
                while (true) {
                    client.SendRequest();
                    Thread.Sleep(10000);
                }
            });

            Console.ReadLine();
        }
    }
}
