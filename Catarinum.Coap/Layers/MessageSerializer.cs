using System.Collections.Generic;
using System.Runtime.Serialization;
using Catarinum.Coap.Util;

namespace Catarinum.Coap.Layers {
    public class MessageSerializer {
        public virtual byte[] Serialize(Message message) {
            var writer = new DatagramWriter();
            writer.Write(Message.Version, Message.VersionBits);
            writer.Write((int) message.Type, Message.TypeBits);
            writer.Write(message.OptionCount, Message.OptionCountBits);
            writer.Write((int) message.Code, Message.CodeBits);
            writer.Write(message.Id, Message.IdBits);
            writer.WriteBytes(GetOptions(message.Options));
            writer.WriteBytes(message.Payload);
            return writer.GetBytes();
        }

        public virtual Message Deserialize(byte[] bytes) {
            var reader = new DatagramReader(bytes);
            var factory = new MessageFactory();
            var version = reader.Read(Message.VersionBits);

            if (version != Message.Version) {
                throw new SerializationException("incorrect version");
            }

            var type = (MessageType) reader.Read(Message.TypeBits);
            var optionCount = reader.Read(Message.OptionCountBits);
            var code = (CodeRegistry) reader.Read(Message.CodeBits);
            var id = reader.Read(Message.IdBits);
            var message = factory.Create(type, code, id);
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

        private static byte[] GetOptions(IEnumerable<Option> options) {
            var writer = new DatagramWriter();
            var lastOptionNumber = 0;

            foreach (var option in options) {
                var delta = (int) option.Number - lastOptionNumber;
                var lenght = option.Value.Length;
                writer.Write(delta, Message.OptionDeltaBits);
                writer.Write(lenght, Message.OptionLengthBits);
                writer.WriteBytes(option.Value);
                lastOptionNumber = (int) option.Number;
            }

            return writer.GetBytes();
        }
    }
}