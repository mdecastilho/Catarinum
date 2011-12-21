using System;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class MessageHelperTests {
        private MessageHelper _helper;
        private Request _request;
        private Response _response;

        [SetUp]
        public void SetUp() {
            _helper = new MessageHelper();
            _request = new Request(0xbc90, CodeRegistry.Get, true);
            _request.AddUri(new Uri("coap://127.0.0.1/temperature"));
            _request.AddToken(Converter.GetBytes(0x71));
            _response = new Response(0x23bb, MessageType.Confirmable, CodeRegistry.Content) { Payload = Converter.GetBytes("22.5 C") };
            _response.AddToken(Converter.GetBytes(0x73));
        }

        [Test]
        public void Should_get_bytes() {
            var bytes = _helper.GetBytes(_request);
            Assert.AreEqual(21, bytes.Length);
        }

        [Test]
        public void Should_get_header() {
            var bytes = _helper.GetBytes(_request);
            var bits = GetBits(bytes, 0, 4);
            Assert.AreEqual("01000010000000011011110010010000", bits);
        }

        [Test]
        public void Should_get_options() {
            var bytes = _helper.GetBytes(_request);
            var uriBits = GetBits(bytes, 4, 12);
            var tokenBits = GetBits(bytes, 16, 5);
            Assert.AreEqual("100110110111010001100101011011010111000001100101011100100110000101110100011101010111001001100101", uriBits);
            Assert.AreEqual("0010010001110001000000000000000000000000", tokenBits);
        }

        [Test]
        public void Should_get_payload() {
            var bytes = _helper.GetBytes(_response);
            var bits = GetBits(bytes, 9, 6);
            Assert.AreEqual("001100100011001000101110001101010010000001000011", bits);
        }

        private static string GetBits(byte[] bytes, int srcOffset, int count) {
            var dst = new byte[count];
            Buffer.BlockCopy(bytes, srcOffset, dst, 0, count);
            var bits = "";

            foreach (var b in dst) {
                bits += Convert.ToString(b, 2).PadLeft(8, '0');
            }

            return bits;
        }
    }
}