using System.Collections.Generic;

namespace Catarinum {
    public abstract class Message {
        private int _version = 1;
        private int _optionCount;
        public int Id { get; private set; }
        public MessageType Type { get; private set; }
        public CodeRegistry Code { get; private set; }
        public byte[] Payload { get; set; }
        public List<Option> Options;
        public string Source { get; set; }
        public string Destination { get; set; }

        public bool IsConfirmable {
            get { return Type.Equals(MessageType.Confirmable); }
        }

        public bool IsNonConfirmable {
            get { return Type.Equals(MessageType.NonConfirmable); }
        }

        public bool IsAcknowledgement {
            get { return Type.Equals(MessageType.Acknowledgement); }
        }

        public bool IsReset {
            get { return Type.Equals(MessageType.Reset); }
        }

        protected Message(int id, MessageType type, CodeRegistry code) {
            Id = id;
            Type = type;
            Code = code;
            Options = new List<Option>();
        }

        public bool MatchToken(byte[] token) {
            return Options.MatchToken(token);
        }
    }
}