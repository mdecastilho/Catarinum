using System.Collections.Generic;

namespace Catarinum.Coap.Helpers {
    public class DatagramHelper {
        private const int ByteSize = 8;
        private List<byte> _bytes;
        private byte _currentByte;
        private int _currentBitIndex;

        public DatagramHelper() {
            _bytes = new List<byte>();
            ResetCurrentByte();
        }

        public void AddBits(int value, int size) {
            for (var i = size - 1; i >= 0; i--) {
                var bit = (value >> i & 1) != 0;

                if (bit) {
                    _currentByte |= (byte) (1 << _currentBitIndex);
                }

                --_currentBitIndex;

                if (_currentBitIndex < 0) {
                    AddCurrentByte();
                }
            }
        }

        public void AddBytes(byte[] bytes) {
            _bytes.AddRange(bytes);
        }

        public byte[] GetBytes() {
            AddCurrentByte();
            var bytes = _bytes.ToArray();
            _bytes = new List<byte>();
            return bytes;
        }

        private void AddCurrentByte() {
            if (_currentBitIndex < ByteSize - 1) {
                _bytes.Add(_currentByte);
                ResetCurrentByte();
            }
        }

        private void ResetCurrentByte() {
            _currentByte = 0;
            _currentBitIndex = ByteSize - 1;
        }
    }
}
