using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class IdSequenceTests {
        private IdSequence _idSequence;

        [SetUp]
        public void SetUp() {
            _idSequence = new IdSequence();
        }

        [Test]
        public void Should_be_sequential() {
            var id = _idSequence.NextId();
            var last = id;

            for (var i = 0; i < 100; i++) {
                id = _idSequence.NextId();
                Assert.AreEqual(last + 1, id);
                last = id;
            }
        }

        [Test]
        public void Should_reset_if_max_id() {
            _idSequence.CurrentId = IdSequence.MaxId;
            var next = _idSequence.NextId();
            Assert.AreEqual(1, next);
        }
    }
}