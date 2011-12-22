using System;

namespace Catarinum.Coap {
    public class Response : Message {
        public bool IsPiggyBacked {
            get { return IsAcknowledgement && Code == CodeRegistry.Content && Payload.Length > 0; }
        }

        public bool IsSeparate {
            get { return !IsPiggyBacked; }
        }

        public Response(int id, MessageType type, CodeRegistry code)
            : base(id, type, code) {

            if (!IsValidCodeRegistry(code)) {
                throw new ArgumentException("Invalid code registry for response.");
            }

            if (IsReset) {
                throw new ArgumentException("Reset message MUST NOT carry response.");
            }
        }

        private static bool IsValidCodeRegistry(CodeRegistry code) {
            return (int) code >= 64 && (int) code <= 191;
        }
    }
}