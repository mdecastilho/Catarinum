using Catarinum.Coap;
using Catarinum.Coap.Layers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class TransmissionContextTests {
        private TimerStub _timer;
        private TransmissionContext _ctx;

        [SetUp]
        public void SetUp() {
            _timer = new TimerStub();
            _ctx = new TransmissionContext(_timer) { Message = new Request(CodeRegistry.Get, true) };
        }

        [Test]
        public void Should_schedule_retransmission() {
            _timer.Ticks = 1;
            _ctx.ScheduleRetry(RetryCallback, ErrorCallback);
            Assert.AreEqual(1, _ctx.Retries);
            Assert.Greater(_ctx.Timeout, 0);
        }

        [Test]
        public void Should_double_timeout_on_each_retransmission() {
            _timer.Ticks = 2;
            _ctx.ScheduleRetry(RetryCallback, ErrorCallback);
            Assert.AreEqual(2, _ctx.Retries);
            Assert.AreEqual(_timer.Timeouts[0] * 2, _timer.Timeouts[1]);
        }

        [Test]
        public void Should_stop_retransmission_if_exceed_max() {
            _timer.Ticks = 10;
            _ctx.ScheduleRetry(RetryCallback, ErrorCallback);
            Assert.AreEqual(MessageLayer.MaxRetransmissions, _ctx.Retries);
        }

        private static void RetryCallback(ITransmissionContext transmissionContext) { }
        private static void ErrorCallback(ITransmissionContext transmissionContext) { }
    }
}