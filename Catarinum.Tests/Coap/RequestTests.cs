using System;
using Catarinum.Coap;
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
        public void Should_set_uri() {
            _request.Uri = new Uri("coap://server/temperature");
            Assert.AreEqual(1, _request.OptionCount);
        }

        [Test]
        public void Should_get_remote_address() {
            _request.Uri = new Uri("coap://server/temperature");
            Assert.AreEqual("server", _request.RemoteAddress);
        }

        [Test]
        public void Should_get_default_port() {
            _request.Uri = new Uri("coap://server/temperature");
            Assert.AreEqual(5683, _request.Port);
        }

        [Test]
        public void Should_get_port() {
            _request.Uri = new Uri("coap://server:8080/temperature");
            Assert.AreEqual(8080, _request.Port);
        }

        [Test]
        public void Should_get_uri_path() {
            _request.Uri = new Uri("coap://server:8080/temperature");
            Assert.AreEqual("/temperature", _request.UriPath);
        }
    }
}