using System.Linq;
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
        public void Should_handle_confirmable_request() {
            var request = CreateRequest();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsAcknowledgement)));
        }

        [Test]
        public void Should_handle_non_confirmable_request() {
            var request = CreateRequest(MessageType.NonConfirmable);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsAcknowledgement)), Times.Never());
        }

        [Test]
        public void Should_send_reset_response_if_context_is_missing() {
            var request = CreateRequest();
            _resourceMock.Setup(r => r.IsContextMissing(It.IsAny<byte[]>())).Returns(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsReset)));
        }

        [Test]
        public void Should_not_send_reset_if_is_non_confirmable() {
            var request = CreateRequest(MessageType.NonConfirmable);
            _resourceMock.Setup(r => r.IsContextMissing(It.IsAny<byte[]>())).Returns(true);
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsReset)), Times.Never());
        }

        [Test]
        public void Should_send_piggy_backed_response() {
            var request = CreateRequestWithPiggyBackedResponse();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsAcknowledgement && m.Code == CodeRegistry.Content)));
        }

        [Test]
        public void Should_not_handle_duplicated_requests() {
            var request = CreateRequest();
            _handler.HandleRequest(request);
            _handler.HandleRequest(request);
            _resourceMock.Verify(r => r.Get(It.IsAny<byte[]>()), Times.Once());
        }

        [Test]
        public void Response_source_should_match_request_destination() {
            var request = CreateRequest();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(r => r.Source.Equals(request.Destination))));
        }

        [Test]
        public void Separate_ack_response_id_should_match_request_id() {
            var request = CreateRequest();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.IsAcknowledgement && m.Id == 0x7d34)));
        }

        [Test]
        public void Separate_response_token_should_match_request_token() {
            var request = CreateRequest();
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Options.FirstOrDefault(o => o.Value.SequenceEqual(Util.GetBytes(0x71))) != null)));
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
            _handler.HandleRequest(request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Options.FirstOrDefault(o => o.Value.SequenceEqual(Util.GetBytes(0x71))) != null)));
        }

        private static Request CreateRequest(MessageType type = MessageType.Confirmable) {
            var request = new Request(0x7d34, type, CodeRegistry.Get) { Destination = "127.0.0.1:50120" };
            request.Options.Add(new Option { Type = OptionType.UriPath, Value = Util.GetBytes("GET /temperature") });
            var token = new Option { Type = OptionType.Token, Value = Util.GetBytes(0x71) };
            request.Options.Add(token);
            return request;
        }

        private Request CreateRequestWithPiggyBackedResponse(MessageType type = MessageType.Confirmable) {
            var request = CreateRequest(type);
            _resourceMock.Setup(r => r.CanGet(It.IsAny<byte[]>())).Returns(true);
            return request;
        }
    }
}
