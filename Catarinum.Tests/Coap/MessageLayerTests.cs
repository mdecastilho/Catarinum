using System.Linq;
using Catarinum.Coap;
using Catarinum.Coap.Layers;
using Moq;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageLayerTests {
        private Mock<ITransmissionContext> _transactionMock;
        private Mock<ITransmissionContextFactory> _transactionFactoryMock;
        private Mock<ILayer> _lowerLayerMock;
        private Mock<IMessageObserver> _observer;
        private MessageLayer _messageLayer;

        [SetUp]
        public void SetUp() {
            _transactionMock = new Mock<ITransmissionContext>();
            _transactionFactoryMock = new Mock<ITransmissionContextFactory>();
            _lowerLayerMock = new Mock<ILayer>();
            _observer = new Mock<IMessageObserver>();
            _messageLayer = new MessageLayer(_lowerLayerMock.Object, _transactionFactoryMock.Object);
            _messageLayer.RegisterObserver(_observer.Object);
            _transactionFactoryMock.Setup(f => f.Create(_messageLayer, It.IsAny<Message>())).Returns(_transactionMock.Object);
        }

        [Test]
        public void Should_send_message_over_lower_layer() {
            var message = new Message(MessageType.Acknowledgement);
            _messageLayer.Send(message);
            _lowerLayerMock.Verify(l => l.Send(message));
        }

        [Test]
        public void Should_set_message_id() {
            var message = new Message(MessageType.Acknowledgement);
            _messageLayer.Send(message);
            _lowerLayerMock.Verify(l => l.Send(It.Is<Message>(m => m.Id > 0)));
        }

        [Test]
        public void Should_schedule_retransmission_if_confirmable() {
            var request = new Request(CodeRegistry.Get, true);
            _messageLayer.Send(request);
            Assert.AreEqual(1, _messageLayer.Retransmissions.Count());
        }

        [Test]
        public void Should_cancel_retransmission_if_reply_received() {
            var request = new Request(CodeRegistry.Get, true);
            _messageLayer.Send(request);
            var ack = new Message(MessageType.Acknowledgement) { Id = request.Id };
            _messageLayer.OnReceive(ack);
            Assert.AreEqual(0, _messageLayer.Retransmissions.Count());
        }

        [Test]
        public void Should_ignore_duplicated_messages() {
            var response = new Response(MessageType.NonConfirmable, CodeRegistry.Content) { Id = 1 };
            _messageLayer.OnReceive(response);
            _messageLayer.OnReceive(response);
            _observer.Verify(h => h.OnReceive(response), Times.Once());
        }

        [Test]
        public void Should_reply_duplicated_requests_from_cache_if_confirmable() {
            var request = new Request(CodeRegistry.Get, true) { Id = 1 };
            var ack = new Message(MessageType.Acknowledgement) { Id = 1 };
            _messageLayer.OnReceive(request);
            _messageLayer.Send(ack);
            _messageLayer.OnReceive(request);
            _observer.Verify(h => h.OnReceive(request), Times.Once());
            _lowerLayerMock.Verify(l => l.Send(ack), Times.Exactly(2));
        }
    }
}