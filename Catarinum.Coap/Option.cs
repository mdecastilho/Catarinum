namespace Catarinum.Coap {
    public class Option {
        public OptionNumber Number { get; private set; }
        public byte[] Value { get; set; }

        public bool IsCritical {
            get { return (int) Number % 2 > 0; }
        }

        public bool IsElective {
            get { return (int) Number % 2 == 0; }
        }

        public OptionFormat Format {
            get { return OptionHelper.GetOptionFormat(Number); }
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