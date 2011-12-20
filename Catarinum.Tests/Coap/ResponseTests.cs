using System;
using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class ResponseTests {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid code registry for response.")]
        public void Should_have_valid_code_registry() {
            new Response(1, MessageType.Confirmable, CodeRegistry.Empty);
        }

        [Test]
        public void Confirmable_message_should_carry_response() {
            var response = new Response(1, MessageType.Confirmable, CodeRegistry.Created);
            Assert.IsNotNull(response);
        }

        [Test]
        public void Non_confirmable_message_message_should_carry_response() {
            var response = new Response(1, MessageType.NonConfirmable, CodeRegistry.Created);
            Assert.IsNotNull(response);
        }

        [Test]
        public void Acknowledgement_message_should_carry_response() {
            var response = new Response(1, MessageType.Acknowledgement, CodeRegistry.Created);
            Assert.IsNotNull(response);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Reset message MUST NOT carry response.")]
        public void Reset_message_should_not_carry_response() {
            new Response(1, MessageType.Reset, CodeRegistry.Created);
        }

        [Test]
        public void Response_should_be_piggy_backed() {
            var response = new Response(1, MessageType.Acknowledgement, CodeRegistry.Content) { Payload = new byte[10] };
            Assert.IsTrue(response.IsPiggyBacked);
        }
    }
}