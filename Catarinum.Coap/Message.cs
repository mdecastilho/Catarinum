using System;
using System.Collections.Generic;
using System.Linq;

namespace Catarinum.Coap {
    public class Message {
        private readonly List<Option> _options;
        public int Id { get; private set; }
        public MessageType Type { get; private set; }
        public CodeRegistry Code { get; private set; }
        public byte[] Payload { get; set; }
        public Uri Uri { get; private set; }

        public byte[] Token {
            get {
                var token = _options.FirstOrDefault(o => o.Number.Equals(OptionNumber.Token));
                return token != null ? token.Value : new Option(OptionNumber.Token).Value;
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

        public IEnumerable<Option> Options {
            get { return _options; }
        }

        public int OptionCount {
            get { return Options.Count(); }
        }

        public Message(int id, MessageType type)
            : this(id, type, CodeRegistry.Empty) {

            if (IsConfirmable) {
                throw new ArgumentException("Confirmable message MUST NOT be empty.");
            }

            if (IsNonConfirmable) {
                throw new ArgumentException("Non-confirmable message MUST NOT be empty.");
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

        public void AddUri(Uri uri) {
            Uri = uri;
            var parser = new CoapUriParser();
            _options.AddRange(parser.GetUriPath(uri));
            _options.AddRange(parser.GetUriQuery(uri));
        }

        public void AddToken(byte[] token) {
            AddOption(new Option(OptionNumber.Token, token));
        }
    }
}