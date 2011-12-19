using System;
using NUnit.Framework;

namespace Catarinum.Tests {
    [TestFixture]
    public class MessageTests {
        [Test]
        public void Confirmable_message_should_carry_request() {
            var message = new Message(1, MessageType.Confirmable, CodeRegistry.Get);
            Assert.IsTrue(message.IsRequest);
        }

        [Test]
        public void Confirmable_message_should_carry_response() {
            var message = new Message(1, MessageType.Confirmable, CodeRegistry.Created);
            Assert.IsTrue(message.IsResponse);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Confirmable message MUST NOT be empty.")]
        public void Confirmable_message_should_not_be_empty() {
            new Message(1, MessageType.Confirmable);
        }

        [Test]
        public void Non_confirmable_message_should_carry_request() {
            var message = new Message(1, MessageType.NonConfirmable, CodeRegistry.Get);
            Assert.IsTrue(message.IsRequest);
        }

        [Test]
        public void Non_confirmable_message_message_should_carry_response() {
            var message = new Message(1, MessageType.NonConfirmable, CodeRegistry.Created);
            Assert.IsTrue(message.IsResponse);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Non-confirmable message MUST NOT be empty.")]
        public void Non_confirmable_message_message_should_not_be_empty() {
            new Message(1, MessageType.NonConfirmable);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Acknowledgement message MUST NOT carry request.")]
        public void Acknowledgement_message_should_not_carry_request() {
            new Message(1, MessageType.Acknowledgement, CodeRegistry.Get);
        }

        [Test]
        public void Acknowledgement_message_should_carry_response() {
            var message = new Message(1, MessageType.Acknowledgement, CodeRegistry.Created);
            Assert.IsTrue(message.IsResponse);
        }

        [Test]
        public void Acknowledgement_message_should_be_empty() {
            var message = new Message(1, MessageType.Acknowledgement);
            Assert.IsTrue(message.IsEmpty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Reset message MUST NOT carry request/response.")]
        public void Reset_message_should_not_carry_request() {
            new Message(1, MessageType.Reset, CodeRegistry.Get);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Reset message MUST NOT carry request/response.")]
        public void Reset_message_should_not_carry_response() {
            new Message(1, MessageType.Reset, CodeRegistry.Get);
        }

        [Test]
        public void Reset_message_should_be_empty() {
            var message = new Message(1, MessageType.Reset);
            Assert.IsTrue(message.IsEmpty);
        }

        [Test]
        public void Acknowledgement_message_should_be_piggy_backed() {
            var message = new Message(1, MessageType.Acknowledgement, CodeRegistry.Content) { Payload = new byte[10] };
            Assert.IsTrue(message.IsPiggyBackedResponse);
        }
    }
}