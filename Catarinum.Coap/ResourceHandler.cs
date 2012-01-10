namespace Catarinum.Coap {
    public class ResourceHandler : IMessageObserver {
        private readonly ILayer _messageLayer;
        private readonly IResource _resource;

        public ResourceHandler(ILayer messageLayer, IResource resource) {
            _messageLayer = messageLayer;
            _resource = resource;
        }

        public void OnSend(Message message) { }

        public void OnReceive(Message message) {
            var request = message as Request;

            if (request != null) {
                var uri = request.GetFirstOption(OptionNumber.UriPath).Value;

                if (CanBePiggyBacked(uri)) {
                    Respond(request, uri, true);
                }
                else {
                    if (request.IsConfirmable) {
                        Accept(request);
                    }

                    Respond(request, uri);
                }
            }
        }

        private bool CanBePiggyBacked(byte[] uri) {
            return _resource.CanGet(uri);
        }

        private void Accept(Request request) {
            var ack = new Message(MessageType.Acknowledgement) { Id = request.Id };
            _messageLayer.Send(ack);
        }

        private void Respond(Request request, byte[] uri, bool isPiggyBacked = false) {
            var type = isPiggyBacked ? MessageType.Acknowledgement : request.Type;
            var code = CodeRegistry.Content;
            var payload = new byte[0];

            try {
                payload = _resource.Get(uri);
            }
            catch (ResponseError error) {
                code = error.Code;
            }

            var response = new Response(type, code) {
                RemoteAddress = request.RemoteAddress,
                Port = request.Port,
                Token = request.Token,
                Payload = payload
            };

            if (isPiggyBacked) {
                response.Id = request.Id;
            }

            _messageLayer.Send(response);
        }
    }
}