using System.IO;

namespace Catarinum.Util {
    public class DatagramReader {
        private const int ByteSize = 8;
        private readonly MemoryStream _bytes;
        private byte _currentByte;
        private int _currentBitIndex;

        public DatagramReader(byte[] bytes) {
            _bytes = new MemoryStream(bytes);
            _currentByte = 0;
            _currentBitIndex = -1;
        }

        public int Read(int numBits) {
            var bits = 0;

            for (var i = numBits - 1; i >= 0; i--) {
                if (_currentBitIndex < 0) {
                    ReadCurrentByte();
                }

                var bit = (_currentByte >> _currentBitIndex & 1) != 0;

                if (bit) {
                    bits |= (1 << i);
                }

                --_currentBitIndex;
            }

            return bits;
        }

        public byte[] ReadBytes(int count) {
            if (count < 0) {
                count = _bytes.Capacity;
            }

            var buffer = new byte[count];
            _bytes.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public byte[] ReadAllBytes() {
            return ReadBytes(-1);
        }

        private void ReadCurrentByte() {
            var val = _bytes.ReadByte();

            if (val >= 0) {
                _currentByte = (byte) val;
            }
            else {
                _currentByte = 0;
            }

            _currentBitIndex = ByteSize - 1;
        }
    }
}