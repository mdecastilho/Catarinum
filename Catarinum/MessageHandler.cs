using System;

namespace Catarinum {
    public class MessageHandler {
        private readonly ISocket _socket;
        private readonly IResourceHandler _resourceHandler;

        public MessageHandler(ISocket socket, IResourceHandler resourceHandler) {
            _socket = socket;
            _resourceHandler = resourceHandler;
        }

        public void HandleRequest(Request request) {
            var response = new Response { Id = request.Id, Type = MessageType.Acknowledgement };

            if (request.Options.ContainsKey(OptionType.Token)) {
                response.Options.Add(OptionType.Token, request.Options[OptionType.Token]);
            }

            try {
                _resourceHandler.GetResource(request.Options[OptionType.Uri]);
            }
            catch (ResponseError error) {
                response.Code = error.Code;
            }
            catch (Exception e) {
                response.Type = MessageType.Reset;
            }


            if (request.Type == MessageType.NonConfirmable) {
                return;
            }

            _socket.Send(response);
        }
    }
}