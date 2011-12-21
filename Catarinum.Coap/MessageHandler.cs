﻿using System.Collections.Generic;

namespace Catarinum.Coap {
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
            var uri = request.GetFirstOption(OptionNumber.UriPath).Value;

            if (!IsDuplicatedRequest(request)) {
                if (_resource.IsContextMissing(uri)) {
                    Reject(request);
                }
                else {
                    if (IsPiggyBacked(uri)) {
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
        }

        private bool IsDuplicatedRequest(Request request) {
            return _messages.Contains(request.Id);
        }

        private bool IsPiggyBacked(byte[] uri) {
            return _resource.CanGet(uri);
        }

        private void Accept(Request request) {
            var ack = new Message(request.Id, MessageType.Acknowledgement);
            _socket.Send(ack);
        }

        private void Reject(Request request) {
            if (request.IsConfirmable) {
                var reset = new Message(request.Id, MessageType.Reset);
                _socket.Send(reset);
            }
        }

        private void Respond(Request request, byte[] uri, bool isPiggyBacked = false) {
            var id = isPiggyBacked ? request.Id : 1;
            var type = isPiggyBacked ? MessageType.Acknowledgement : request.Type;
            var response = new Response(id, type, CodeRegistry.Content);

            try {
                response.Payload = _resource.Get(uri);
            }
            catch (ResponseError error) {
                response = new Response(id, type, error.Code);
            }

            response.AddToken(request.Token);
            _socket.Send(response);
        }
    }
}