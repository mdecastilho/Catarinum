using Catarinum.Coap;
using Moq;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class MessageHandlerTests {
        private Mock<IMessageLayer> _messageLayerMock;
        private Mock<IResource> _resourceMock;
        private MessageHandler _handler;

        [SetUp]
        public void SetUp() {
            _messageLayerMock = new Mock<IMessageLayer>();
            _resourceMock = new Mock<IResource>();
            _handler = new MessageHandler(_messageLayerMock.Object, _resourceMock.Object);
            Examples.ResourceMock = _resourceMock;
        }

        [Test]
        public void Should_send_piggy_backed_response() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.IsPiggyBacked)));
        }

        [Test]
        public void Piggy_backed_response_id_should_match_request_id() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Id == 0x7d34)));
        }

        [Test]
        public void Piggy_backed_response_token_should_match_request_token() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response_with_token();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Token.Equals(request.Token))));
        }

        [Test]
        public void Should_send_separate_response() {
            var request = Examples.Basic_get_request_causing_a_separate_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.IsSeparate)));
        }

        [Test]
        public void Separate_response_should_send_ack_if_confirmable_request() {
            var request = Examples.Basic_get_request_causing_a_separate_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Message>(m => m.IsAcknowledgement)));
        }

        [Test]
        public void Separate_response_ack_id_should_match_request_id() {
            var request = Examples.Basic_get_request_causing_a_separate_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Message>(m => m.Id == 0x7d38)));
        }

        [Test]
        public void Separate_response_token_should_match_request_token() {
            var request = Examples.Basic_get_request_causing_a_separate_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.Token.Equals(request.Token))));
        }

        [Test]
        public void Should_send_non_confirmable_request() {
            var request = Examples.Basic_get_request_where_the_request_and_response_are_non_confirmable();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Message>(m => m.IsAcknowledgement)), Times.Never());
        }

        [Test]
        public void Should_not_handle_duplicated_requests() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            _handler.HandleRequest(request);
            _handler.HandleRequest(request);
            _resourceMock.Verify(r => r.Get(It.IsAny<byte[]>()), Times.Once());
        }

        [Test]
        public void Response_source_should_have_request_destination() {
            var request = Examples.Basic_get_request_causing_a_piggy_backed_response();
            _handler.HandleRequest(request);
            _messageLayerMock.Verify(l => l.Send(It.Is<Response>(r => r.RemoteAddress.Equals(request.RemoteAddress))));
        }
    }
}
