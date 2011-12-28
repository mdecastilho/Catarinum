﻿using System.Collections.Generic;

namespace Catarinum.Coap.Impl {
    public class RequestHandler : IMessageHandler {
        private readonly IMessageLayer _messageLayer;
        private readonly IResource _resource;
        private readonly List<int> _messages;

        public RequestHandler(IMessageLayer messageLayer, IResource resource) {
            _messageLayer = messageLayer;
            _resource = resource;
            _messages = new List<int>();
        }

        public void Handle(Message message) {
            HandleRequest((Request) message);
        }

        private void HandleRequest(Request request) {
            var uri = request.GetFirstOption(OptionNumber.UriPath).Value;

            if (!IsDuplicated(request)) {
                if (CanBePiggyBacked(uri)) {
                    Respond(request, uri, true);
                }
                else {
                    if (request.IsConfirmable) {
                        Accept(request);
                    }

                    Respond(request, uri);
                }

                _messages.Add(request.Id);
            }
        }

        private bool IsDuplicated(Request request) {
            return _messages.Contains(request.Id);
        }

        private bool CanBePiggyBacked(byte[] uri) {
            return _resource.CanGet(uri);
        }

        private void Accept(Request request) {
            var ack = new Message(request.Id, MessageType.Acknowledgement);
            _messageLayer.Send(ack);
        }

        private void Respond(Request request, byte[] uri, bool isPiggyBacked = false) {
            var id = isPiggyBacked ? request.Id : 1;
            var type = isPiggyBacked ? MessageType.Acknowledgement : request.Type;
            var code = CodeRegistry.Content;
            var payload = new byte[0];

            try {
                payload = _resource.Get(uri);
            }
            catch (ResponseError error) {
                code = error.Code;
            }

            var response = new Response(id, type, code) {
                RemoteAddress = request.RemoteAddress,
                Port = request.Port,
                Token = request.Token,
                Payload = payload
            };

            _messageLayer.Send(response);
        }
    }
}