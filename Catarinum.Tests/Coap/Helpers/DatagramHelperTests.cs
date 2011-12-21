using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class DatagramHelperTests {
        private DatagramHelper _helper;

        [SetUp]
        public void SetUp() {
            _helper = new DatagramHelper();
        }

        [Test]
        public void Should_add_8_bits() {
            _helper.AddBits(1, 2);
            _helper.AddBits(1, 2);
            _helper.AddBits(0, 4);
            var bytes = _helper.GetBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(new byte[] { 80 }, bytes);
        }

        [Test]
        public void Should_add_16_bits() {
            _helper.AddBits(1, 2);
            _helper.AddBits(0, 2);
            _helper.AddBits(2, 4);
            _helper.AddBits(69, 8);
            var bytes = _helper.GetBytes();
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(new byte[] { 66, 69 }, bytes);
        }

        [Test]
        public void Should_add_bytes() {
            _helper.AddBytes(new byte[] { 0, 255 });
            var bytes = _helper.GetBytes();
            Assert.AreEqual(2, bytes.Length);
        }
    }
}