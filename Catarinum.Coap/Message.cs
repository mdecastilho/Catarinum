using System.Collections.Generic;
using System.Linq;

namespace Catarinum.Coap {
    public abstract class Message {
        private readonly List<Option> _options;
        public int Id { get; private set; }
        public MessageType Type { get; private set; }
        public CodeRegistry Code { get; private set; }
        public byte[] Payload { get; set; }
        public string RemoteAddress { get; set; }

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

        public IEnumerable<Option> Options {
            get { return _options; }
        }

        public int OptionCount {
            get { return Options.Count(); }
        }

        public Option Token {
            get {
                var token = _options.FirstOrDefault(o => o.Number.Equals(OptionNumber.Token));
                return token ?? new Option(OptionNumber.Token);
            }
        }

        protected Message(int id, MessageType type, CodeRegistry code) {
            Id = id;
            Type = type;
            Code = code;
            _options = new List<Option>();
        }

        public void AddOption(Option option) {
            _options.Add(option);
        }

        public bool MatchToken(Message message) {
            return Options.MatchToken(message.Token.Value);
        }
    }
}