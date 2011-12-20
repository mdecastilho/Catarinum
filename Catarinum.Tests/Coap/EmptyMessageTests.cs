using System;
using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class EmptyMessageTests {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Confirmable message MUST NOT be empty.")]
        public void Confirmable_message_should_not_be_empty() {
            new EmptyMessage(1, MessageType.Confirmable);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Non-confirmable message MUST NOT be empty.")]
        public void Non_confirmable_message_message_should_not_be_empty() {
            new EmptyMessage(1, MessageType.NonConfirmable);
        }

        [Test]
        public void Acknowledgement_message_should_be_empty() {
            var message = new EmptyMessage(1, MessageType.Acknowledgement);
            Assert.IsNotNull(message);
        }

        [Test]
        public void Reset_message_should_be_empty() {
            var message = new EmptyMessage(1, MessageType.Reset);
            Assert.IsNotNull(message);
        }
    }
}