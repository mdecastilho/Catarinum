using NUnit.Framework;

namespace Catarinum.Tests {
    [TestFixture]
    public class MessageTests {
        [Test]
        public void Default_type_should_be_confirmable() {
            var message = new Message();
            Assert.AreEqual(MessageType.Confirmable, message.Type);
        }
    }
}