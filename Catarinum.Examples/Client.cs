using System;
using Catarinum.Coap;

namespace Catarinum.Examples {
    public class Client {
        public static void Main(string[] args) {
            var uri = new Uri("coap://127.0.0.1:5683/temperature");
            var request = new Request(1, CodeRegistry.Get, true);
        }
    }
}
