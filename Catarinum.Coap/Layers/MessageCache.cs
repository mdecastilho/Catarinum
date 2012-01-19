using System.Collections.Generic;

namespace Catarinum.Coap.Layers {
    public class MessageCache {
        private readonly Dictionary<string, Message> _cache;

        public MessageCache() {
            _cache = new Dictionary<string, Message>();
        }

        public bool ContainsMessage(Message message) {
            return _cache.ContainsKey(message.GetKey());
        }

        public void Add(Message message) {
            _cache.Add(message.GetKey(), message);
        }

        public Message Get(Message message) {
            return _cache[message.GetKey()];
        }
    }
}