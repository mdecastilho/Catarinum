using Moq;
using NUnit.Framework;

namespace Catarinum.Tests {
    [TestFixture]
    public class MessageHandlerTests {
        private Mock<ISocket> _socketMock;
        private Mock<IResource> _resourceMock;
        private MessageHandler _handler;

        [SetUp]
        public void SetUp() {
            _socketMock = new Mock<ISocket>();
            _resourceMock = new Mock<IResource>();
            _handler = new MessageHandler(_socketMock.Object, _resourceMock.Object);
        }

        [Test]
        public void Should_send_ack_if_confirmable_request() {
            var request = CreateRequest(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<EmptyMessage>(m => m.IsAcknowledgement)));
        }

        [Test]
        public void Should_not_send_ack_if_non_confirmable_request() {
            var request = CreateRequest(false);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<EmptyMessage>(m => m.IsAcknowledgement)), Times.Never());
        }

        [Test]
        public void Should_send_reset_if_context_is_missing() {
            var request = CreateRequest(true);
            _resourceMock.Setup(r => r.IsContextMissing(It.IsAny<byte[]>())).Returns(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<EmptyMessage>(m => m.IsReset)));
        }

        [Test]
        public void Should_not_send_reset_if_is_non_confirmable() {
            var request = CreateRequest(false);
            _resourceMock.Setup(r => r.IsContextMissing(It.IsAny<byte[]>())).Returns(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<EmptyMessage>(m => m.IsReset)), Times.Never());
        }

        [Test]
        public void Should_send_piggy_backed_response() {
            var request = CreateRequestWithPiggyBackedResponse();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsPiggyBacked)));
        }

        [Test]
        public void Should_not_handle_duplicated_requests() {
            var request = CreateRequest(true);
            _handler.HandleRequest(request);
            _handler.HandleRequest(request);
            _resourceMock.Verify(r => r.Get(It.IsAny<byte[]>()), Times.Once());
        }

        [Test]
        public void Response_source_should_match_request_destination() {
            var request = CreateRequest(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(r => r.Source.Equals(request.Destination))));
        }

        [Test]
        public void Separate_response_ack_id_should_match_request_id() {
            var request = CreateRequest(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<EmptyMessage>(m => m.Id == 0x7d34)));
        }

        [Test]
        public void Separate_response_token_should_match_request_token() {
            var request = CreateRequest(true);
            var token = Util.GetBytes(0x71);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.MatchToken(token))));
        }

        [Test]
        public void Piggy_backed_response_id_should_match_request_id() {
            var request = CreateRequestWithPiggyBackedResponse();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Id == 0x7d34)));
        }

        [Test]
        public void Piggy_backed_response_token_should_match_request_token() {
            var request = CreateRequestWithPiggyBackedResponse();
            var token = Util.GetBytes(0x71);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.MatchToken(token))));
        }

        private static Request CreateRequest(bool confirmable) {
            var request = new Request(0x7d34, CodeRegistry.Get, confirmable) { Destination = "127.0.0.1:50120" };
            request.Options.Add(new Option { Type = OptionType.UriPath, Value = Util.GetBytes("GET /temperature") });
            var token = new Option { Type = OptionType.Token, Value = Util.GetBytes(0x71) };
            request.Options.Add(token);
            return request;
        }

        private Request CreateRequestWithPiggyBackedResponse() {
            var request = CreateRequest(true);
            _resourceMock.Setup(r => r.CanGet(It.IsAny<byte[]>())).Returns(true);
            _resourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(new byte[10]);
            return request;
        }
    }
}
