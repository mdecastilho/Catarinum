using System;
using System.Collections.Generic;

namespace Catarinum {
    public class Message {
        // Implementations of this specification MUST set this field
        // to 1.  Other values are reserved for future versions.
        private int _version = 1;
        private int _optionCount;
        public int Id { get; private set; }
        public MessageType Type { get; private set; }
        public MessageCode Code { get; private set; }
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

        public bool IsEmpty {
            get { return (int) Code == 0; }
        }

        public bool IsRequest {
            get { return (int) Code >= 1 && (int) Code <= 31; }
        }

        public bool IsResponse {
            get { return (int) Code >= 64 && (int) Code <= 191; }
        }

        public bool IsPiggyBackedResponse {
            get { return IsAcknowledgement && IsResponse && Payload.Length > 0; }
        }

        public Message(int id, MessageType type, MessageCode code = MessageCode.Empty) {
            Id = id;
            Type = type;
            Code = code;
            Options = new List<Option>();
            Validate();
        }

        private void Validate() {
            if (IsConfirmable && IsEmpty) {
                throw new ArgumentException("Confirmable message MUST NOT be empty.");
            }

            if (IsNonConfirmable && IsEmpty) {
                throw new ArgumentException("Non-confirmable message MUST NOT be empty.");
            }

            if (IsAcknowledgement && IsRequest) {
                throw new ArgumentException("Acknowledgement message MUST NOT carry request.");
            }

            if (IsReset && (IsRequest || IsResponse)) {
                throw new ArgumentException("Reset message MUST NOT carry request/response.");
            }
        }

        // If the Path MTU is not known for a destination,
        // an IP MTU of 1280 bytes SHOULD be assumed; if nothing is known about
        // the size of the headers, good upper bounds are 1152 bytes for the
        // message size and 1024 bytes for the payload size.

        //public byte[] Header { get; set; }
        //public byte[] Options { get; set; }
        //
    }
}