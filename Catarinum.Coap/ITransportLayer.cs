using System;

namespace Catarinum.Coap {
    public interface ITransportLayer {
        void Send(Message message);
        void Receive(Action<Message> callback);
    }
}