using System;
using Catarinum.Coap;

namespace Catarinum.Tests.Coap {
    public class BlockExamples {
        // example 1 (figure 2): Simple blockwise GET
        public static Request Simple_blockwise_get() {
            var uri = new Uri("coap://server/status");
            var request = new Request(CodeRegistry.Get, true) { Uri = uri };
            return request;
        }

        public static Response Simple_blockwise_get_block(int num) {
            var m = num < 2 ? 1 : 0;
            var block = new Response(MessageType.Confirmable, CodeRegistry.Content) { Id = 1234, RemoteAddress = "client", Port = 8080, Payload = new byte[128] };
            block.AddOption(new BlockOption(OptionNumber.Block2, num, m, BlockOption.EncodeSzx(128)));
            return block;
        }

        // example 2 (figure 3): Blockwise GET with early negotiation
        public static Request Blockwise_get_with_early_negotiation(int num) {
            var uri = new Uri("coap://server/status");
            var request = new Request(CodeRegistry.Get, true) { Uri = uri };
            request.AddOption(new BlockOption(OptionNumber.Block2, num, 0, BlockOption.EncodeSzx(64)));
            return request;
        }

        public static Response Blockwise_get_with_early_negotiation_response() {
            return new Response(MessageType.Confirmable, CodeRegistry.Content) { Id = 1234, RemoteAddress = "client", Port = 8080, Payload = new byte[384] };
        }
    }
}