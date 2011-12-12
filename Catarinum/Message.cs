using System.Collections.Generic;

namespace Catarinum {
    public class Message {
        public int Id { get; set; }
        public MessageType Type { get; set; }
        public Dictionary<OptionType, byte[]> Options;

        public Message() {
            Options = new Dictionary<OptionType, byte[]>();
        }

        //public byte[] Header { get; set; }
        //public byte[] Options { get; set; }
        //public byte[] Payload { get; set; }
    }

    public enum OptionType {
        Uri,
        ContentType,
        Token
    }
}