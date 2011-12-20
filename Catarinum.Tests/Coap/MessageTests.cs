using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageTests {
        private TestMessage _message;

        [SetUp]
        public void SetUp() {
            _message = new TestMessage();
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
        public void Should_get_token() {
            var token = Util.GetBytes(0x71);
            _message.AddOption(new Option(OptionNumber.Token, token));
            Assert.AreEqual(token, _message.Token.Value);
        }

        [Test]
        public void Should_get_empty_token_if_not_set() {
            Assert.AreEqual(new byte[0], _message.Token.Value);
        }

        [Test]
        public void Should_match_token() {
            var token = Util.GetBytes(0x71);
            _message.AddOption(new Option(OptionNumber.Token, token));
            var message = new TestMessage();
            message.AddOption(new Option(OptionNumber.Token, token));
            Assert.IsTrue(_message.MatchToken(message));
        }
    }

    public class TestMessage : Message {
        public TestMessage()
            : base(1, MessageType.Confirmable, CodeRegistry.Get) {
        }
    }
}
