using Catarinum.Coap;
using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class MessageHelperTests {
        private MessageHelper _helper;

        [SetUp]
        public void SetUp() {
            _helper = new MessageHelper();
        }

        [Test]
        public void Should_get_bytes() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _helper.GetBytes(request);
            Assert.AreEqual(16, bytes.Length);
        }

        [Test]
        public void Should_get_header() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _helper.GetBytes(request);
            var bits = Util.GetBits(bytes, 0, 4);
            Assert.AreEqual("01000001000000010111110100110100", bits);
        }

        [Test]
        public void Should_get_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _helper.GetBytes(request);
            var bits = Util.GetBits(bytes, 4, 12);
            Assert.AreEqual("100110110111010001100101011011010111000001100101011100100110000101110100011101010111001001100101", bits);
        }

        [Test]
        public void Should_get_two_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            var bytes = _helper.GetBytes(request);
            var bits = Util.GetBits(bytes, 16, 5);
            Assert.AreEqual(21, bytes.Length);
            Assert.AreEqual("0010010000100000000000000000000000000000", bits);
        }

        [Test]
        public void Should_get_payload() {
            var response = new Response(0x7d34, MessageType.Acknowledgement, CodeRegistry.Content) {
                Payload = Converter.GetBytes("22.5 C")
            };
            var bytes = _helper.GetBytes(response);
            var bits = Util.GetBits(bytes, 4, 6);
            Assert.AreEqual("001100100011001000101110001101010010000001000011", bits);
        }
    }
}