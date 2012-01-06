using System;
using System.Linq;
using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public class Request : Message {
        private Uri _uri;

        public Request(CodeRegistry code, bool confirmable)
            : base(confirmable ? MessageType.Confirmable : MessageType.NonConfirmable, code) {

            if (!IsValidCodeRegistry(code)) {
                throw new ArgumentException("Invalid code registry for request.");
            }
        }

        public Uri Uri {
            get { return _uri; }
            set {
                _uri = value;
                var parser = new UriParser(_uri);
                RemoteAddress = parser.GetRemoteAddress();
                Port = parser.GetPort();
                AddOptions(parser.GetUriPath());
                AddOptions(parser.GetUriQuery());
            }
        }

        public string UriPath {
            get {
                var uriPath = "";
                var options = Options.Where(o => o.Number == (int) OptionNumber.UriPath);

                foreach (var option in options) {
                    uriPath += string.Format("/{0}", ByteConverter.GetString(option.Value));
                }

                return uriPath;
            }
        }

        public static bool IsValidCodeRegistry(CodeRegistry code) {
            return (int) code >= 1 && (int) code <= 31;
        }
    }
}