using Catarinum.Coap;
using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class BlockOptionTests {
        [Test]
        public void Should_construct_block2_option() {
            var option = new BlockOption(OptionNumber.Block2, 2, 0, BlockOption.EncodeSzx(32));
            Assert.AreEqual(33, ByteConverter.GetInt(option.Value));
            Assert.AreEqual(2, option.Num);
            Assert.AreEqual(0, option.M);
            Assert.AreEqual(32, BlockOption.DecodeSzx(option.Szx));
        }

        [Test]
        public void Should_construct_block1_option() {
            var option = new BlockOption(OptionNumber.Block1, 3, 1, BlockOption.EncodeSzx(128));
            Assert.AreEqual(59, ByteConverter.GetInt(option.Value));
            Assert.AreEqual(3, option.Num);
            Assert.AreEqual(1, option.M);
            Assert.AreEqual(128, BlockOption.DecodeSzx(option.Szx));
        }

        [Test]
        public void Should_encode_szx() {
            var szx1 = BlockOption.EncodeSzx(16);
            var szx2 = BlockOption.EncodeSzx(1024);
            Assert.AreEqual(0, szx1);
            Assert.AreEqual(6, szx2);
        }

        [Test]
        public void Should_decode_szx() {
            var blockSize1 = BlockOption.DecodeSzx(1);
            var blockSize2 = BlockOption.DecodeSzx(3);
            Assert.AreEqual(32, blockSize1);
            Assert.AreEqual(128, blockSize2);
        }
    }
}