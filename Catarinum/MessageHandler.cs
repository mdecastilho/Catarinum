using System.Collections.Generic;
using System.Linq;

namespace Catarinum {
    public class MessageHandler {
        private readonly ISocket _socket;
        private readonly IResource _resource;
        private readonly List<int> _messages;

        public MessageHandler(ISocket socket, IResource resource) {
            _socket = socket;
            _resource = resource;
            _messages = new List<int>();
        }

        public void HandleRequest(Request request) {
            var uri = request.Options.FirstOrDefault(o => o.Type == OptionType.Uri).Value;

            if (!_messages.Contains(request.Id)) {
                if (_resource.IsContextMissing(uri)) {
                    if (request.IsConfirmable) {
                        var reset = new Response(request.Id, MessageType.Reset) { Source = request.Destination };
                        _socket.Send(reset);
                    }
                }
                else {
                    if (IsPiggyBacked(uri)) {
                        HandleResource(request, uri, true);
                    }
                    else {
                        if (request.IsConfirmable) {
                            var ack = new Response(request.Id, MessageType.Acknowledgement) { Source = request.Destination };
                            _socket.Send(ack);
                        }

                        HandleResource(request, uri);
                    }

                    _messages.Add(request.Id);
                }
            }
        }

        private bool IsPiggyBacked(byte[] uri) {
            return _resource.CanGet(uri);
        }

        private void HandleResource(Request request, byte[] uri, bool isPiggyBacked = false) {
            var id = isPiggyBacked ? request.Id : 1;
            var type = isPiggyBacked ? MessageType.Acknowledgement : request.Type;
            var response = new Response(id, type, MessageCode.Content) { Source = request.Destination };

            try {
                response.Payload = _resource.Get(uri);
            }
            catch (ResponseError error) {
                response = new Response(id, type, error.Code);
            }

            var token = request.Options.FirstOrDefault(o => o.Type == OptionType.Token);

            if (token != null) {
                response.Options.Add(token);
            }

            _socket.Send(response);
        }
    }
}