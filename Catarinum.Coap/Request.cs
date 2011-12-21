using System;
using Catarinum.Coap.Helpers;

namespace Catarinum.Coap {
    public class Request : Message {
        public Uri Uri { get; private set; }

        public Request(int id, CodeRegistry code, bool confirmable)
            : base(id, confirmable ? MessageType.Confirmable : MessageType.NonConfirmable, code) {

            if (!IsValidCodeRegistry(code)) {
                throw new ArgumentException("Invalid code registry for request.");
            }
        }

        public void AddUri(Uri uri) {
            Uri = uri;
            var parser = new CoapUriParser();
            AddOptions(parser.GetUriPath(uri));
            AddOptions(parser.GetUriQuery(uri));
        }

        private static bool IsValidCodeRegistry(CodeRegistry code) {
            return (int) code >= 1 && (int) code <= 31;
        }
    }
}