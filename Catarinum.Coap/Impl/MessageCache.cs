using System.Collections.Generic;

namespace Catarinum.Coap.Impl {
    public class MessageCache {
        private readonly Dictionary<string, Message> _cache;

        public MessageCache() {
            _cache = new Dictionary<string, Message>();
        }

        public bool ContainsMessage(Message message) {
            var key = GetKey(message);
            return _cache.ContainsKey(key);
        }

        public void Add(Message message) {
            var key = GetKey(message);
            _cache.Add(key, message);
        }

        public Message Get(Message message) {
            var key = GetKey(message);
            return _cache[key];
        }

        private static string GetKey(Message message) {
            return string.Format("{0}:{1}-{2}", message.RemoteAddress, message.Port, message.Id);
        }
    }
}