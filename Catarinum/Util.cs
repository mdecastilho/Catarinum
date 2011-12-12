using System;
using System.Text;

namespace Catarinum {
    public class Util {
        public static byte[] GetBytes(string s) {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] GetBytes(int value) {
            return BitConverter.GetBytes(value);
        }
    }
}