using System;

namespace Catarinum.Tests.Coap {
    public class Util {
        public static string GetBits(byte[] bytes, int srcOffset, int count) {
            var dst = new byte[count];
            Buffer.BlockCopy(bytes, srcOffset, dst, 0, count);
            var bits = "";

            foreach (var b in dst) {
                bits += Convert.ToString(b, 2).PadLeft(8, '0');
            }

            return bits;
        }
    }
}