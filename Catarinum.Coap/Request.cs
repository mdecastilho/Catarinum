using System;
using Catarinum.Coap.Helpers;

namespace Catarinum.Coap {
    public class Request : Message {
        public Request(int id, CodeRegistry code, bool confirmable)
            : base(id, confirmable ? MessageType.Confirmable : MessageType.NonConfirmable, code) {

            if (!IsValidCodeRegistry(code)) {
                throw new ArgumentException("Invalid code registry for request.");
            }
        }

        public static bool IsValidCodeRegistry(CodeRegistry code) {
            return (int) code >= 1 && (int) code <= 31;
        }

        public void AddUri(Uri uri) {
            var port = 5683;

            if (!uri.IsDefaultPort) {
                port = uri.Port;
            }

            RemoteAddress = string.Format("{0}:{1}", uri.Host, port);
            var parser = new CoapUriParser();
            AddOptions(parser.GetUriPath(uri));
            AddOptions(parser.GetUriQuery(uri));
        }
    }
}