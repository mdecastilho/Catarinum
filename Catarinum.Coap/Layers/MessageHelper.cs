using System;
using Catarinum.Coap.Util;

namespace Catarinum.Coap.Layers {
    public static class MessageHelper {
        public static string GetKey(this Message message) {
            return string.Format("{0}:{1}-{2}", message.RemoteAddress, message.Port, message.Id);
        }

        public static string GetTransactionKey(this Message message) {
            return ByteConverter.GetString(message.Token);
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