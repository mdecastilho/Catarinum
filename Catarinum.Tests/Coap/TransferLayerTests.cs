using Catarinum.Coap;
using Catarinum.Coap.Layers;
using Moq;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class TransferLayerTests {
        private Mock<ILayer> _lowerLayerMock;
        private Mock<IMessageObserver> _observer;
        private TransferLayer _transferLayer;

        [SetUp]
        public void SetUp() {
            _lowerLayerMock = new Mock<ILayer>();
            _observer = new Mock<IMessageObserver>();
            _transferLayer = new TransferLayer(_lowerLayerMock.Object, 128);
            _transferLayer.RegisterObserver(_observer.Object);

            var id = 1234;

            _lowerLayerMock.Setup(l => l.Send(It.IsAny<Message>())).Callback<Message>(m => {
                m.Id = id;
                id++;
            });
        }

        [Test]
        public void Should_receive_response_in_blocks() {
            Simple_blockwise_get_request();
            _observer.Verify(o => o.OnReceive(It.Is<Response>(r => r.Payload.Length == 384)));
        }

        [Test]
        public void Should_request_next_response_block() {
            Simple_blockwise_get_request();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => r.GetFirstOption(OptionNumber.Block2) != null && ((BlockOption) r.GetFirstOption(OptionNumber.Block2)).Num == 1)));
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => r.GetFirstOption(OptionNumber.Block2) != null && ((BlockOption) r.GetFirstOption(OptionNumber.Block2)).Num == 2)));
        }

        [Test]
        public void Should_early_negotiate_response_block_size() {
            Blockwise_get_with_early_negotiation();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Payload.Length == 64)), Times.Exactly(6));
        }

        [Test]
        public void Should_late_negotiate_response_block_size() {
            Blockwise_get_with_late_negotiation();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Payload.Length == 128)), Times.Once());
            _lowerLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Payload.Length == 64)), Times.Exactly(4));
        }

        [Test]
        public void Should_send_request_in_blocks() {
            Simple_atomic_blockwise_put();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(block => block.Payload.Length == 128)), Times.Exactly(3));
        }

        [Test]
        public void Should_negotiate_request_block_size() {
            Simple_atomic_blockwise_put_with_negotiation();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => r.Payload.Length == 128)), Times.Once());
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => r.Payload.Length == 32)), Times.Exactly(3));
        }

        // block examples
        private void Simple_blockwise_get_request() {
            var request = BlockExamples.Simple_blockwise_get();
            _transferLayer.Send(request);

            for (var i = 0; i < 3; i++) {
                var block = BlockExamples.Simple_blockwise_get_block(i);
                block.Request = request;
                _transferLayer.OnReceive(block);
            }
        }

        private void Blockwise_get_with_early_negotiation() {
            _observer.Setup(o => o.OnReceive(It.IsAny<Message>())).Callback<Message>(m => {
                var response = BlockExamples.Blockwise_get_with_early_negotiation_response();
                response.Request = (Request) m;
                _transferLayer.Send(response);
            });

            for (var i = 0; i < 6; i++) {
                _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_early_negotiation(i));
            }
        }

        private void Blockwise_get_with_late_negotiation() {
            _observer.Setup(o => o.OnReceive(It.IsAny<Message>())).Callback<Message>(m => {
                var response = BlockExamples.Blockwise_get_with_late_negotiation_response();
                response.Request = (Request) m;
                _transferLayer.Send(response);
            });

            _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_late_negotiation(0));
            _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_late_negotiation(2));
            _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_late_negotiation(3));
            _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_late_negotiation(4));
            _transferLayer.OnReceive(BlockExamples.Blockwise_get_with_late_negotiation(5));
        }

        private void Simple_atomic_blockwise_put() {
            _transferLayer.Send(BlockExamples.Simple_atomic_blockwise_put());

            for (var i = 0; i < 3; i++) {
                _transferLayer.OnReceive(BlockExamples.Simple_atomic_blockwise_put_response(i));
            }
        }

        private void Simple_atomic_blockwise_put_with_negotiation() {
            _transferLayer.Send(BlockExamples.Simple_atomic_blockwise_put_with_negotiation());
            _transferLayer.OnReceive(BlockExamples.Simple_atomic_blockwise_put_with_negotiation_response(0));
            _transferLayer.OnReceive(BlockExamples.Simple_atomic_blockwise_put_with_negotiation_response(4));
            _transferLayer.OnReceive(BlockExamples.Simple_atomic_blockwise_put_with_negotiation_response(5));
            _transferLayer.OnReceive(BlockExamples.Simple_atomic_blockwise_put_with_negotiation_response(6));
        }
    }
}