using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class DatagramWriterTests {
        private DatagramWriter _writer;

        [SetUp]
        public void SetUp() {
            _writer = new DatagramWriter();
        }

        [Test]
        public void Should_write_8_bits() {
            _writer.Write(1, 2);
            _writer.Write(1, 2);
            _writer.Write(0, 4);
            var bytes = _writer.GetBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(new byte[] { 80 }, bytes);
        }

        [Test]
        public void Should_write_16_bits() {
            _writer.Write(1, 2);
            _writer.Write(0, 2);
            _writer.Write(2, 4);
            _writer.Write(69, 8);
            var bytes = _writer.GetBytes();
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(new byte[] { 66, 69 }, bytes);
        }

        [Test]
        public void Should_write_bytes() {
            _writer.WriteBytes(new byte[] { 0, 255 });
            var bytes = _writer.GetBytes();
            Assert.AreEqual(2, bytes.Length);
        }
    }
}