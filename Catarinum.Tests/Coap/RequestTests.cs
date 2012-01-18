using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class RequestTests {
        private Request _request;

        [SetUp]
        public void SetUp() {
            _request = new Request(CodeRegistry.Get, true);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid code registry for request.")]
        public void Should_have_valid_code_registry() {
            new Request(CodeRegistry.Empty, true);
        }

        [Test]
        public void Confirmable_message_should_carry_request() {
            Assert.IsNotNull(_request);
        }

        [Test]
        public void Non_confirmable_message_should_carry_request() {
            var request = new Request(CodeRegistry.Get, false);
            Assert.IsNotNull(request);
        }

        [Test]
        public void Should_get_uri() {
            var request = new Request(CodeRegistry.Get, false) { RemoteAddress = "server" };
            request.AddOption(new Option(OptionNumber.UriPath, ByteConverter.GetBytes("temperature")));
            Assert.AreEqual("coap://server/temperature", request.Uri.ToString());
            Assert.AreEqual(1, request.OptionCount);
        }
    }
}