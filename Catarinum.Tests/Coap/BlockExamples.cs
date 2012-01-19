using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;

namespace Catarinum.Tests.Coap {
    public class BlockExamples {
        // example 1 (figure 2): Simple blockwise GET
        public static Request Simple_blockwise_get() {
            var uri = new Uri("coap://server/status");
            var request = new Request(CodeRegistry.Get, true) { Id = 1234, Uri = uri };
            return request;
        }

        public static Response Simple_blockwise_get_block(int num) {
            var id = 1234 + num;
            var m = num < 2 ? 1 : 0;
            var block = new Response(MessageType.Acknowledgement, CodeRegistry.Content) { Id = id, Payload = new byte[128] };
            block.AddOption(new BlockOption(OptionNumber.Block2, num, m, BlockOption.EncodeSzx(128)));
            return block;
        }

        // example 2 (figure 3): Blockwise GET with early negotiation
        public static Request Blockwise_get_with_early_negotiation(int num) {
            var id = 1234 + num;
            var uri = new Uri("coap://server/status");
            var request = new Request(CodeRegistry.Get, true) { Id = id, Uri = uri };
            request.AddOption(new BlockOption(OptionNumber.Block2, num, 0, BlockOption.EncodeSzx(64)));
            return request;
        }

        public static Response Blockwise_get_with_early_negotiation_response() {
            return new Response(MessageType.Acknowledgement, CodeRegistry.Content) { Id = 1234, Payload = new byte[384] };
        }

        // example 3 (figure 4): Blockwise GET with late negotiation
        public static Request Blockwise_get_with_late_negotiation(int num) {
            var id = 1234 + num;
            var uri = new Uri("coap://server/status");
            var request = new Request(CodeRegistry.Get, true) { Id = id, Uri = uri };

            if (num > 0) {
                request.AddOption(new BlockOption(OptionNumber.Block2, num, 0, BlockOption.EncodeSzx(64)));
            }

            return request;
        }

        public static Response Blockwise_get_with_late_negotiation_response() {
            return new Response(MessageType.Acknowledgement, CodeRegistry.Content) { Id = 1234, Payload = new byte[384] };
        }

        // example 6 (figure 7): Simple atomic blockwise PUT
        public static Request Simple_atomic_blockwise_put() {
            var uri = new Uri("coap://server/options");
            var request = new Request(CodeRegistry.Put, true) { Id = 1234, Uri = uri, Token = ByteConverter.GetBytes(0x17), Payload = new byte[384] };
            return request;
        }

        public static Response Simple_atomic_blockwise_put_response(int num) {
            var m = num < 2 ? 1 : 0;
            var response = new Response(MessageType.Acknowledgement, CodeRegistry.Changed) { Id = 1234, Token = ByteConverter.GetBytes(0x17) };
            response.AddOption(new BlockOption(OptionNumber.Block1, num, m, BlockOption.EncodeSzx(128)));
            return response;
        }

        // example 7 (figure 9): Simple atomic blockwise PUT with negotiation
        public static Request Simple_atomic_blockwise_put_with_negotiation() {
            var uri = new Uri("coap://server/options");
            var request = new Request(CodeRegistry.Put, true) { Id = 1234, Uri = uri, Token = ByteConverter.GetBytes(0x17), Payload = new byte[224] };
            return request;
        }

        public static Response Simple_atomic_blockwise_put_with_negotiation_response(int num) {
            var m = num < 6 ? 1 : 0;
            var response = new Response(MessageType.Acknowledgement, CodeRegistry.Changed) { Id = 1234, Token = ByteConverter.GetBytes(0x17) };
            response.AddOption(new BlockOption(OptionNumber.Block1, num, m, BlockOption.EncodeSzx(32)));
            return response;
        }
    }
}