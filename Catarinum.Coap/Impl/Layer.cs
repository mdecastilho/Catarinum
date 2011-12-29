using System.Collections.Generic;

namespace Catarinum.Coap.Impl {
    public abstract class Layer : ILayer, IHandler {
        private readonly List<IHandler> _handlers;

        protected Layer() {
            _handlers = new List<IHandler>();
        }

        public abstract void Send(Message message);

        public virtual void Handle(Message message) {
            Deliver(message);
        }

        protected void Deliver(Message message) {
            foreach (var handler in _handlers) {
                handler.Handle(message);
            }
        }

        public virtual void AddHandler(IHandler handler) {
            _handlers.Add(handler);
        }
    }
}