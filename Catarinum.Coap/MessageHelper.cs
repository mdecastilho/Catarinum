using System;
using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public static class MessageHelper {
        public static string GetKey(this Message message) {
            return string.Format("{0}:{1}-{2}", message.RemoteAddress, message.Port, message.Id);
        }

        public static string GetTransactionKey(this Message message) {
            return ByteConverter.GetString(message.Token);
        }

        public static Message GetBlock(this Message message, int num, int szx) {
            var blockSize = BlockOption.DecodeSzx(szx);
            var payloadOffset = num * blockSize;
            var payloadLeft = message.Payload.Length - payloadOffset;

            if (payloadLeft > 0) {
                var block = message is Request
                    ? (Message) new Request(message.Code, message.Type == MessageType.Confirmable) { Id = message.Id }
                    : new Response(message.Type, message.Code) { Id = message.Id };

                foreach (var option in message.Options) {
                    block.AddOption(option);
                }

                var m = blockSize < payloadLeft ? 1 : 0;

                if (m == 0) {
                    blockSize = payloadLeft;
                }

                var payload = new byte[blockSize];
                Buffer.BlockCopy(message.Payload, payloadOffset, payload, 0, blockSize);
                block.Payload = payload;
                var optionNumber = message is Request ? OptionNumber.Block1 : OptionNumber.Block2;
                block.AddOption(new BlockOption(optionNumber, num, m, szx));
                return block;
            }

            return null;
        }

        public static Message AppendPayload(this Message message, byte[] bytes) {
            var newPayload = new byte[message.Payload.Length + bytes.Length];
            Buffer.BlockCopy(message.Payload, 0, newPayload, 0, message.Payload.Length);
            Buffer.BlockCopy(bytes, 0, newPayload, message.Payload.Length, bytes.Length);
            message.Payload = newPayload;
            return message;
        }
    }
}