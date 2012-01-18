using System.Collections.Generic;

namespace Catarinum.Coap {
    public class Option {
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

        public OptionNumber Number { get; private set; }
        public byte[] Value { get; set; }

        public bool IsCritical {
            get { return (int) Number % 2 > 0; }
        }

        public bool IsElective {
            get { return (int) Number % 2 == 0; }
        }

        public OptionFormat Format {
            get { return Formats[Number]; }
        }

        public Option(OptionNumber number)
            : this(number, new byte[0]) {
        }

        public Option(OptionNumber number, byte[] value) {
            Number = number;
            Value = value;
        }
    }
}