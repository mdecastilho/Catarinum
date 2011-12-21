using System.Collections.Generic;

namespace Catarinum.Coap.Helpers {
    internal static class OptionHelper {
        // this should go somewhere else
        private static readonly Dictionary<OptionNumber, OptionFormat> Formats = new Dictionary<OptionNumber, OptionFormat> {
            { OptionNumber.ContentType, OptionFormat.Uint },
            { OptionNumber.MaxAge, OptionFormat.Uint },
            { OptionNumber.ProxyUri, OptionFormat.String },
            { OptionNumber.ETag, OptionFormat.Opaque },
            { OptionNumber.UriHost, OptionFormat.String },
            { OptionNumber.LocationPath, OptionFormat.String },
            { OptionNumber.UriPort, OptionFormat.Uint },
            { OptionNumber.LocationQuery, OptionFormat.String },
            { OptionNumber.UriPath, OptionFormat.String },
            { OptionNumber.Token, OptionFormat.Opaque },
            { OptionNumber.Accept, OptionFormat.Uint },
            { OptionNumber.IfMatch, OptionFormat.Uint },
            { OptionNumber.UriQuery, OptionFormat.String },
            { OptionNumber.IfNoneMatch, OptionFormat.Uint },
        };

        public static OptionFormat GetOptionFormat(OptionNumber number) {
            return Formats[number];
        }
    }
}