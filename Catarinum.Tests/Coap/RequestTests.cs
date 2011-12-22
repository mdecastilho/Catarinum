using System;
using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class RequestTests {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid code registry for request.")]
        public void Should_have_valid_code_registry() {
            new Request(1, CodeRegistry.Empty, true);
        }

        [Test]
        public void Confirmable_message_should_carry_request() {
            var request = new Request(1, CodeRegistry.Get, true);
            Assert.IsNotNull(request);
        }

        [Test]
        public void Non_confirmable_message_should_carry_request() {
            var request = new Request(1, CodeRegistry.Get, false);
            Assert.IsNotNull(request);
        }

        [Test]
        public void Should_add_uri() {
            var request = new Request(1, CodeRegistry.Get, true);
            var uri = new Uri("coap://server/temperature");
            request.AddUri(uri);
            Assert.AreEqual(1, request.OptionCount);
        }

        [Test]
        public void Should_get_uri() {
            var request = new Request(1, CodeRegistry.Get, true);
            var uri = new Uri("coap://server/temperature");
            request.AddUri(uri);
            Assert.AreEqual(uri, request.Uri);
        }
    }
}