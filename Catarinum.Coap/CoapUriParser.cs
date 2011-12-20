using System;
using System.Collections.Generic;
using System.Linq;

namespace Catarinum.Coap {
    public class CoapUriParser {
        public string GetDestinationAddress(Uri uri) {
            return uri.Host;
        }

        public int GetDestinationPort(Uri uri) {
            return uri.Port > 0 ? uri.Port : 5683;
        }

        public IEnumerable<Option> GetUriPath(Uri uri) {
            if (uri.AbsolutePath.Equals("/")) {
                return new List<Option>();
            }

            var paths = uri.AbsolutePath.Split('/').Skip(1);
            return paths.Select(p => new Option(OptionNumber.UriPath, Util.GetBytes(p)));
        }

        public IEnumerable<Option> GetUriQuery(Uri uri) {
            if (uri.Query.Equals(string.Empty)) {
                return new List<Option>();
            }

            var paths = uri.Query.Split('&');
            return paths.Select(p => new Option(OptionNumber.UriQuery, Util.GetBytes(p)));
        }
    }
}