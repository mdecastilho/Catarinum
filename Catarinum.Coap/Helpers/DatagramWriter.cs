using System.IO;

namespace Catarinum.Coap.Helpers {
    public class DatagramWriter {
        private const int ByteSize = 8;
        private MemoryStream _bytes;
        private byte _currentByte;
        private int _currentBitIndex;

        public DatagramWriter() {
            _bytes = new MemoryStream();
            ResetCurrentByte();
        }

        public void Write(int value, int numBits) {
            for (var i = numBits - 1; i >= 0; i--) {
                var bit = (value >> i & 1) != 0;

                if (bit) {
                    _currentByte |= (byte) (1 << _currentBitIndex);
                }

                --_currentBitIndex;

                if (_currentBitIndex < 0) {
                    WriteCurrentByte();
                }
            }
        }

        public void WriteBytes(byte[] bytes) {
            _bytes.Write(bytes, 0, bytes.Length);
        }

        public byte[] GetBytes() {
            WriteCurrentByte();
            var bytes = _bytes.ToArray();
            _bytes = new MemoryStream();
            return bytes;
        }

        private void WriteCurrentByte() {
            if (_currentBitIndex < ByteSize - 1) {
                _bytes.WriteByte(_currentByte);
                ResetCurrentByte();
            }
        }

        private void ResetCurrentByte() {
            _currentByte = 0;
            _currentBitIndex = ByteSize - 1;
        }
    }
}
