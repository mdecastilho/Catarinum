using System;
using Catarinum.Coap.Util;

namespace Catarinum.Coap {
    public class BlockOption : Option {
        public int Num {
            get { return ByteConverter.GetInt(Value) >> 4; }
            set { Value = Encode(value, Szx, M); }
        }

        public int M {
            get { return ByteConverter.GetInt(Value) >> 3 & 0x01; }
            set { Value = Encode(Num, Szx, value); }
        }

        public int Szx {
            get { return ByteConverter.GetInt(Value) & 0x7; }
            set { Value = Encode(Num, value, M); }
        }

        public int BlockSize {
            get { return DecodeSzx(Szx); }
            set { Szx = EncodeSzx(value); }
        }

        public BlockOption(OptionNumber number)
            : base(number) {
        }

        public BlockOption(OptionNumber number, byte[] value)
            : base(number, value) {
        }

        public BlockOption(OptionNumber number, int num, int m, int szx)
            : base(number, Encode(num, m, szx)) {
        }

        public static int EncodeSzx(int blockSize) {
            return (int) ((Math.Log(blockSize) / Math.Log(2)) - 4);
        }

        public static int DecodeSzx(int szx) {
            return 1 << (szx + 4);
        }

        private static byte[] Encode(int num, int m, int szx) {
            var value = 0;
            value |= szx & 0x7;
            value |= m << 3;
            value |= num << 4;
            return ByteConverter.GetBytes(value);
        }
    }
}