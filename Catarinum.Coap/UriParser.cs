using System;
using System.Collections.Generic;
using System.Linq;
using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public class UriParser {
        private readonly Uri _uri;

        public UriParser(Uri uri) {
            _uri = uri;
        }

        public string GetRemoteAddress() {
            return _uri.Host;
        }

        public int GetPort() {
            return _uri.IsDefaultPort ? 5683 : _uri.Port;
        }

        public IEnumerable<Option> GetUriPath() {
            if (_uri.AbsolutePath.Equals("/")) {
                return new List<Option>();
            }

            var paths = _uri.AbsolutePath.Split('/').Skip(1);
            return paths.Select(p => new Option(OptionNumber.UriPath, ByteConverter.GetBytes(p)));
        }

        public IEnumerable<Option> GetUriQuery() {
            if (_uri.Query.Equals(string.Empty)) {
                return new List<Option>();
            }

            var paths = _uri.Query.Split('&');
            return paths.Select(p => new Option(OptionNumber.UriQuery, ByteConverter.GetBytes(p)));
        }
    }
}