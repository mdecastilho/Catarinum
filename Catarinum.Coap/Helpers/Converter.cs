using System;
using System.Text;

namespace Catarinum.Coap.Helpers {
    public class Converter {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static byte[] GetBytes(string s) {
            return Encoding.GetBytes(s);
        }

        public static byte[] GetBytes(int value) {
            return BitConverter.GetBytes(value);
        }

        public static string GetString(byte[] bytes) {
            return Encoding.GetString(bytes);
        }

        public static string GetHexString(byte[] bytes) {
            return BitConverter.ToString(bytes);
        }
    }
}