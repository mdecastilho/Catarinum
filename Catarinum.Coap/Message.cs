using System;
using System.Collections.Generic;
using System.Linq;

namespace Catarinum.Coap {
    public class Message {
        public const int Version = 1;
        public const int VersionBits = 2;
        public const int TypeBits = 2;
        public const int OptionCountBits = 4;
        public const int CodeBits = 8;
        public const int IdBits = 16;
        public const int OptionDeltaBits = 4;
        public const int OptionLengthBits = 4;
        private readonly List<Option> _options;
        public int Id { get; set; }
        public MessageType Type { get; private set; }
        public CodeRegistry Code { get; private set; }
        public byte[] Payload { get; set; }
        public string RemoteAddress { get; set; }
        public int Port { get; set; }

        public byte[] Token {
            get {
                var token = GetFirstOption(OptionNumber.Token);
                return token != null ? token.Value : new byte[0];
            }
            set {
                AddOption(new Option(OptionNumber.Token, value));
            }
        }

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

        public bool IsEmpty {
            get { return Code.Equals(CodeRegistry.Empty); }
        }

        public bool IsReply {
            get { return IsAcknowledgement || IsReset; }
        }

        public IEnumerable<Option> Options {
            get { return _options; }
        }

        public int OptionCount {
            get { return Options.Count(); }
        }

        public Message(MessageType type)
            : this(type, CodeRegistry.Empty) {

            if (IsConfirmable) {
                throw new ArgumentException("Confirmable message MUST NOT be empty.");
            }

            if (IsNonConfirmable) {
                throw new ArgumentException("Non-confirmable message MUST NOT be empty.");
            }
        }

        protected Message(MessageType type, CodeRegistry code) {
            Type = type;
            Code = code;
            Payload = new byte[0];
            _options = new List<Option>();
        }

        public void AddOption(Option option) {
            _options.Add(option);
        }

        public Option GetFirstOption(OptionNumber number) {
            return _options.FirstOrDefault(o => o.Number == (int) number);
        }

        protected void AddOptions(IEnumerable<Option> options) {
            _options.AddRange(options);
        }
    }
}