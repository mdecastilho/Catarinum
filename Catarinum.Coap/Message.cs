using System;
using System.Collections.Generic;
using System.Linq;
using Catarinum.Coap.Helpers;

namespace Catarinum.Coap {
    public class Message {
        private readonly List<Option> _options;
        public int Id { get; private set; }
        public MessageType Type { get; private set; }
        public CodeRegistry Code { get; private set; }
        public byte[] Payload { get; set; }

        public byte[] Token {
            get {
                var token = GetFirstOption(OptionNumber.Token);
                return token != null ? token.Value : new byte[0];
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
            Payload = new byte[0];
            _options = new List<Option>();
        }

        public void AddOption(Option option) {
            _options.Add(option);
        }

        public void AddToken(byte[] token) {
            AddOption(new Option(OptionNumber.Token, token));
        }

        public Option GetFirstOption(OptionNumber number) {
            return _options.FirstOrDefault(o => o.Number == (int) number);
        }

        public byte[] GetBytes() {
            var helper = new MessageHelper();
            return helper.GetBytes(this);
        }

        protected void AddOptions(IEnumerable<Option> options) {
            _options.AddRange(options);
        }
    }
}