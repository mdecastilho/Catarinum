using Catarinum.Coap;
using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class TokenManagerTests {
        private TokenManager _tokenManager;

        [SetUp]
        public void SetUp() {
            _tokenManager = new TokenManager();
        }

        [Test]
        public void Should_be_sequential() {
            var id = ByteConverter.GetInt(_tokenManager.AcquireToken());
            var last = id;

            for (var i = 0; i < 100; i++) {
                id = ByteConverter.GetInt(_tokenManager.AcquireToken());
                Assert.AreEqual(last + 1, id);
                last = id;
            }
        }
    }
}