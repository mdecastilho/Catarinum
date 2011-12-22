using Catarinum.Coap;
using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class MessageSerializerTests {
        [Test]
        public void Should_serialize() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = MessageSerializer.Serialize(request);
            Assert.AreEqual(16, bytes.Length);
        }

        [Test]
        public void Should_serialize_header() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = MessageSerializer.Serialize(request);
            var bits = Util.GetBits(bytes, 0, 4);
            Assert.AreEqual("01000001000000010111110100110100", bits);
        }

        [Test]
        public void Should_serialize_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = MessageSerializer.Serialize(request);
            var bits = Util.GetBits(bytes, 4, 12);
            Assert.AreEqual("100110110111010001100101011011010111000001100101011100100110000101110100011101010111001001100101", bits);
        }

        [Test]
        public void Should_serialize_two_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            var bytes = MessageSerializer.Serialize(request);
            var bits = Util.GetBits(bytes, 16, 5);
            Assert.AreEqual(21, bytes.Length);
            Assert.AreEqual("0010010000100000000000000000000000000000", bits);
        }

        [Test]
        public void Should_serialize_payload() {
            var response = new Response(0x7d34, MessageType.Acknowledgement, CodeRegistry.Content) { Payload = ByteConverter.GetBytes("22.5 C") };
            var bytes = MessageSerializer.Serialize(response);
            var bits = Util.GetBits(bytes, 4, 6);
            Assert.AreEqual("001100100011001000101110001101010010000001000011", bits);
        }

        [Test]
        public void Should_unserialize() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            var bytes = MessageSerializer.Serialize(request);
            var message = MessageSerializer.Unserialize(bytes);
            Assert.IsNotNull(message);
            Assert.IsInstanceOf<Request>(message);
            Assert.AreEqual(2, message.OptionCount);
        }
    }
}