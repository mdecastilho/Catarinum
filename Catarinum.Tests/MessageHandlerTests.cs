using System;
using Moq;
using NUnit.Framework;

namespace Catarinum.Tests {
    [TestFixture]
    public class MessageHandlerTests {
        private MessageHandler _handler;
        private Mock<ISocket> _socketMock;
        private Mock<IResourceHandler> _resourceHandlerMock;
        private Request _request;

        [SetUp]
        public void SetUp() {
            _socketMock = new Mock<ISocket>();
            _resourceHandlerMock = new Mock<IResourceHandler>();
            _handler = new MessageHandler(_socketMock.Object, _resourceHandlerMock.Object);
            _request = new Request { Id = 0x7d34 };
            _request.Options.Add(OptionType.Uri, Util.GetBytes("GET /temperature"));
        }

        [Test]
        public void Should_handle_confirmable_request() {
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Type == MessageType.Acknowledgement)));
        }

        [Test]
        public void Should_handle_non_confirmable_request() {
            _request.Type = MessageType.NonConfirmable;
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.IsAny<Response>()), Times.Never());
        }

        [Test]
        public void Response_should_have_same_message_id() {
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Id == 0x7d34)));
        }

        [Test]
        public void Response_should_have_same_token() {
            var token = Util.GetBytes(0x71);
            _request.Options.Add(OptionType.Token, token);
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Options[OptionType.Token] == token)));
        }

        [Test]
        public void Response_type_should_be_reset_in_case_of_error() {
            _resourceHandlerMock.Setup(h => h.GetResource(It.IsAny<byte[]>())).Throws(new ArgumentException("error"));
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Type == MessageType.Reset)));
        }

        [Test]
        public void Should_send_piggy_backed_response() {
            _request.Id = 0xbc90;
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Type == MessageType.Acknowledgement && m.Code == ResponseCode.Content)));
        }

        [Test]
        public void Should_send_piggy_backed_response_if_not_found() {
            _request.Id = 0xbc91;
            _resourceHandlerMock.Setup(h => h.GetResource(It.IsAny<byte[]>())).Throws(new ResponseError(ResponseCode.NotFound));
            _handler.HandleRequest(_request);
            _socketMock.Verify(s => s.Send(It.Is<Response>(m => m.Type == MessageType.Acknowledgement && m.Code == ResponseCode.NotFound)));
        }
    }
}
