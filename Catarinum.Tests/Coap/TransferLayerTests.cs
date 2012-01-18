using Catarinum.Coap;
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
            _transferLayer = new TransferLayer(_lowerLayerMock.Object, 512);
            _transferLayer.RegisterObserver(_observer.Object);

            var id = 1234;

            _lowerLayerMock.Setup(l => l.Send(It.IsAny<Message>())).Callback<Message>(m => {
                m.Id = id;
                id++;
            });
        }

        //[Test]
        //public void Should_send_message_in_blocks() {
        //    var response = new Response(MessageType.Confirmable, CodeRegistry.Content) { Payload = new byte[32] };
        //    _transferLayer.Send(response);
        //    _lowerLayerMock.Verify(l => l.Send(It.Is<Response>(block => block.Payload.Length == 16)));
        //}

        [Test]
        public void Should_receive_message_in_blocks() {
            Simple_blockwise_get_request();
            _observer.Verify(o => o.OnReceive(It.Is<Response>(r => r.Payload.Length == 384)));
        }

        [Test]
        public void Should_request_next_block() {
            Simple_blockwise_get_request();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => RequestedBlock(r, 1))));
            _lowerLayerMock.Verify(l => l.Send(It.Is<Request>(r => RequestedBlock(r, 2))));
        }

        [Test]
        public void Should_negotiate_block_size() {
            Blockwise_get_with_early_negotiation();
            _lowerLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Payload.Length == 64)), Times.Exactly(6));
        }

        private static bool RequestedBlock(Request request, int num) {
            return request.GetFirstOption(OptionNumber.Block2) != null
                && ((BlockOption) request.GetFirstOption(OptionNumber.Block2)).Num == num;
        }

        private void Simple_blockwise_get_request() {
            _transferLayer.Send(BlockExamples.Simple_blockwise_get());

            for (var i = 0; i < 3; i++) {
                _transferLayer.OnReceive(BlockExamples.Simple_blockwise_get_block(i));
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
    }
}