using System;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageTests {
        private Message _message;

        [SetUp]
        public void SetUp() {
            _message = new Message(1, MessageType.Acknowledgement);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Confirmable message MUST NOT be empty.")]
        public void Confirmable_message_should_not_be_empty() {
            new Message(1, MessageType.Confirmable);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Non-confirmable message MUST NOT be empty.")]
        public void Non_confirmable_message_message_should_not_be_empty() {
            new Message(1, MessageType.NonConfirmable);
        }

        [Test]
        public void Acknowledgement_message_should_be_empty() {
            var message = new Message(1, MessageType.Acknowledgement);
            Assert.IsNotNull(message);
        }

        [Test]
        public void Reset_message_should_be_empty() {
            var message = new Message(1, MessageType.Reset);
            Assert.IsNotNull(message);
        }

        [Test]
        public void Should_add_options() {
            _message.AddOption(new Option(OptionNumber.ContentType));
            _message.AddOption(new Option(OptionNumber.UriPath));
            _message.AddOption(new Option(OptionNumber.Token));
            Assert.AreEqual(3, _message.OptionCount);
        }

        [Test]
        public void Should_add_same_option_number_twice() {
            _message.AddOption(new Option(OptionNumber.Token));
            _message.AddOption(new Option(OptionNumber.Token));
            Assert.AreEqual(2, _message.OptionCount);
        }

        [Test]
        public void Should_add_token() {
            _message.AddToken(Converter.GetBytes(0x71));
            Assert.AreEqual(1, _message.OptionCount);
        }

        [Test]
        public void Should_get_token() {
            var token = Converter.GetBytes(0x71);
            _message.AddToken(token);
            Assert.AreEqual(token, _message.Token);
        }

        [Test]
        public void Should_get_empty_token_if_not_set() {
            Assert.AreEqual(new byte[0], _message.Token);
        }

        [Test]
        public void Should_get_first_option() {
            var token1 = Converter.GetBytes(0x71);
            var token2 = Converter.GetBytes(0x72);
            _message.AddToken(token1);
            _message.AddToken(token2);
            Assert.AreEqual(token1, _message.GetFirstOption(OptionNumber.Token).Value);
        }

        [Test]
        public void Should_get_bytes() {
            var bytes = _message.GetBytes();
            Assert.AreEqual(4, bytes.Length);
        }
    }
}
