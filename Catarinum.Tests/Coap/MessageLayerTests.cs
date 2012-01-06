using System.Linq;
using Catarinum.Coap;
using Catarinum.Coap.Impl;
using Moq;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageLayerTests {
        private Mock<ITransaction> _transactionMock;
        private Mock<ITransactionFactory> _transactionFactoryMock;
        private Mock<ILayer> _lowerLayerMock;
        private MessageLayer _messageLayer;

        [SetUp]
        public void SetUp() {
            _transactionMock = new Mock<ITransaction>();
            _transactionFactoryMock = new Mock<ITransactionFactory>();
            _lowerLayerMock = new Mock<ILayer>();
            _messageLayer = new MessageLayer(_lowerLayerMock.Object, _transactionFactoryMock.Object);
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
        public void Should_add_transaction_if_confirmable() {
            var request = new Request(CodeRegistry.Get, true);
            _messageLayer.Send(request);
            Assert.AreEqual(1, _messageLayer.Transactions.Count());
        }

        [Test]
        public void Should_remove_transaction_if_reply_received() {
            var request = new Request(CodeRegistry.Get, true);
            _messageLayer.Send(request);
            var ack = new Message(MessageType.Acknowledgement) { Id = request.Id };
            _messageLayer.Handle(ack);
            Assert.AreEqual(0, _messageLayer.Transactions.Count());
        }
    }
}