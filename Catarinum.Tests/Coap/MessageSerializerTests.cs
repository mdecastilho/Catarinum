using System.Runtime.Serialization;
using Catarinum.Coap;
using Catarinum.Coap.Layers;
using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageSerializerTests {
        private MessageSerializer _serializer;

        [SetUp]
        public void SetUp() {
            _serializer = new MessageSerializer();
        }

        [Test]
        public void Should_serialize() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _serializer.Serialize(request);
            Assert.AreEqual(16, bytes.Length);
        }

        [Test]
        public void Should_serialize_header() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _serializer.Serialize(request);
            var bits = Util.GetBits(bytes, 0, 4);
            Assert.AreEqual("01000001000000010111110100110100", bits);
        }

        [Test]
        public void Should_serialize_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            var bytes = _serializer.Serialize(request);
            var bits = Util.GetBits(bytes, 4, 12);
            Assert.AreEqual("100110110111010001100101011011010111000001100101011100100110000101110100011101010111001001100101", bits);
        }

        [Test]
        public void Should_serialize_two_options() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            var bytes = _serializer.Serialize(request);
            var bits = Util.GetBits(bytes, 16, 5);
            Assert.AreEqual(21, bytes.Length);
            Assert.AreEqual("0010010000100000000000000000000000000000", bits);
        }

        [Test]
        public void Should_serialize_payload() {
            var response = new Response(MessageType.Acknowledgement, CodeRegistry.Content) { Payload = ByteConverter.GetBytes("22.5 C") };
            var bytes = _serializer.Serialize(response);
            var bits = Util.GetBits(bytes, 4, 6);
            Assert.AreEqual("001100100011001000101110001101010010000001000011", bits);
        }

        [Test]
        public void Should_deserialize() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            var bytes = _serializer.Serialize(request);
            var message = _serializer.Deserialize(bytes);
            Assert.IsNotNull(message);
            Assert.IsInstanceOf<Request>(message);
            Assert.AreEqual(2, message.OptionCount);
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Should_throw_exception_when_version_is_incorrect() {
            var writer = new DatagramWriter();
            writer.Write(0, Message.VersionBits);
            writer.Write((int) MessageType.Confirmable, Message.TypeBits);
            writer.Write(0, Message.OptionCountBits);
            writer.Write((int) CodeRegistry.Get, Message.CodeBits);
            writer.Write(1, Message.IdBits);
            var bytes = writer.GetBytes();
            _serializer.Deserialize(bytes);
        }
    }
}