using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class DatagramReaderTests {
        [Test]
        public void Should_read_8_bits() {
            var reader = new DatagramReader(new byte[] { 80 });
            Assert.AreEqual(1, reader.Read(2));
            Assert.AreEqual(1, reader.Read(2));
            Assert.AreEqual(0, reader.Read(4));
        }

        [Test]
        public void Should_read_16_bits() {
            var reader = new DatagramReader(new byte[] { 66, 69 });
            Assert.AreEqual(66, reader.Read(8));
            Assert.AreEqual(69, reader.Read(8));
        }

        [Test]
        public void Should_read_bytes() {
            var reader = new DatagramReader(new byte[] { 0, 255 });
            Assert.AreEqual(0, reader.Read(4));
            Assert.AreEqual(0, reader.Read(4));
            Assert.AreEqual(new byte[] { 255 }, reader.ReadAllBytes());
        }
    }
}