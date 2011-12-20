using NUnit.Framework;

namespace Catarinum.Tests {
    [TestFixture]
    public class OptionTests {
        [Test]
        public void Should_be_critical() {
            var option = new Option(OptionNumber.ContentType);
            Assert.IsTrue(option.IsCritical);
        }

        [Test]
        public void Should_be_elective() {
            var option = new Option(OptionNumber.MaxAge);
            Assert.IsTrue(option.IsElective);
        }

        [Test]
        public void Should_get_format() {
            var option = new Option(OptionNumber.Token);
            Assert.AreEqual(OptionFormat.Opaque, option.Format);
        }
    }
}