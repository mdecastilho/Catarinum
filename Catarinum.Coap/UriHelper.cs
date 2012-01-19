using System;
using System.Collections.Generic;
using System.Linq;
using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public static class UriHelper {
        public static Uri GetUri(this Request request) {
            var uriPath = "";
            var options = request.Options.Where(o => o.Number == OptionNumber.UriPath);

            foreach (var option in options) {
                uriPath += string.Format("/{0}", ByteConverter.GetString(option.Value));
            }

            return new Uri(string.Format("coap://{0}:{1}{2}", request.RemoteAddress, request.Port, uriPath));
        }

        public static void SetUri(this Request request, Uri uri) {
            request.RemoteAddress = uri.Host;
            request.Port = uri.IsDefaultPort ? 5683 : uri.Port;
            request.AddOptions(OptionNumber.UriPath, ExtractUriPathOptions(uri));
            request.AddOptions(OptionNumber.UriQuery, ExtractUriQueryOptions(uri));
        }

        public static IEnumerable<Option> ExtractUriPathOptions(Uri uri) {
            if (uri.AbsolutePath.Equals("/")) {
                return new List<Option>();
            }

            var paths = uri.AbsolutePath.Split('/').Skip(1);
            return paths.Select(p => new Option(OptionNumber.UriPath, ByteConverter.GetBytes(p)));
        }

        public static IEnumerable<Option> ExtractUriQueryOptions(Uri uri) {
            if (uri.Query.Equals(string.Empty)) {
                return new List<Option>();
            }

            var paths = uri.Query.Split('&');
            return paths.Select(p => new Option(OptionNumber.UriQuery, ByteConverter.GetBytes(p)));
        }
    }
}