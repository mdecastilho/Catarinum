using Catarinum.Coap.Helpers;

namespace Catarinum.Coap {
    public class Option {
        public int Number { get; private set; }
        public byte[] Value { get; set; }

        public bool IsCritical {
            get { return Number % 2 > 0; }
        }

        public bool IsElective {
            get { return Number % 2 == 0; }
        }

        public OptionFormat Format {
            get { return OptionHelper.GetOptionFormat((OptionNumber) Number); }
        }

        public Option(OptionNumber number)
            : this(number, new byte[0]) {
        }

        public Option(OptionNumber number, byte[] value) {
            Number = (int) number;
            Value = value;
        }
    }
}