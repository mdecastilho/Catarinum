using Catarinum.Util;

namespace Catarinum.Coap {
    public class MessageSerializer {
        public static byte[] Serialize(Message message) {
            var writer = new DatagramWriter();
            writer.Write(Message.Version, Message.VersionBits);
            writer.Write((int) message.Type, Message.TypeBits);
            writer.Write(message.OptionCount, Message.OptionCountBits);
            writer.Write((int) message.Code, Message.CodeBits);
            writer.Write(message.Id, Message.IdBits);
            writer.WriteBytes(GetOptions(message));
            writer.WriteBytes(message.Payload);
            return writer.GetBytes();
        }

        public static Message Unserialize(byte[] bytes) {
            var reader = new DatagramReader(bytes);
            var version = reader.Read(Message.VersionBits);
            var type = (MessageType) reader.Read(Message.TypeBits);
            var optionCount = reader.Read(Message.OptionCountBits);
            var code = (CodeRegistry) reader.Read(Message.CodeBits);
            var id = reader.Read(Message.IdBits);
            var message = CreateMessage(code, id, type);
            var currentOption = 0;

            for (var i = 0; i < optionCount; i++) {
                var delta = reader.Read(Message.OptionDeltaBits);
                var length = reader.Read(Message.OptionLengthBits);
                currentOption += delta;
                var option = new Option((OptionNumber) currentOption) { Value = reader.ReadBytes(length) };
                message.AddOption(option);
            }

            message.Payload = reader.ReadAllBytes();
            return message;
        }

        private static byte[] GetOptions(Message message) {
            var writer = new DatagramWriter();
            var lastOptionNumber = 0;

            foreach (var option in message.Options) {
                var delta = option.Number - lastOptionNumber;
                var lenght = option.Value.Length;
                writer.Write(delta, Message.OptionDeltaBits);
                writer.Write(lenght, Message.OptionLengthBits);
                writer.WriteBytes(option.Value);
                lastOptionNumber = option.Number;
            }

            return writer.GetBytes();
        }

        private static Message CreateMessage(CodeRegistry code, int id, MessageType type) {
            if (Request.IsValidCodeRegistry(code)) {
                return new Request(id, code, type == MessageType.Confirmable);
            }

            return Response.IsValidCodeRegistry(code)
                ? new Response(id, type, code)
                : new Message(id, type);
        }
    }
}