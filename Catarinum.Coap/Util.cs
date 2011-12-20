using System;
using System.Text;

namespace Catarinum.Coap {
    public class Util {
        public static byte[] GetBytes(string s) {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] GetBytes(int value) {
            return BitConverter.GetBytes(value);
        }

        public static string GetString(byte[] bytes) {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string GetHexString(byte[] bytes) {
            return BitConverter.ToString(bytes);
        }
    }
}