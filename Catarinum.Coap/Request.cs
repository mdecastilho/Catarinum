using System;

namespace Catarinum.Coap {
    public class Request : Message {
        public Uri Uri {
            get { return this.GetUri(); }
            set { this.SetUri(value); }
        }

        public Request(CodeRegistry code, bool confirmable)
            : base(confirmable ? MessageType.Confirmable : MessageType.NonConfirmable, code) {

            if (!IsValidCodeRegistry(code)) {
                throw new ArgumentException("Invalid code registry for request.");
            }
        }

        public static bool IsValidCodeRegistry(CodeRegistry code) {
            return (int) code >= 1 && (int) code <= 31;
        }
    }
}