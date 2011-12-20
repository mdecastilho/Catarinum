using System;

namespace Catarinum.Coap {
    public class EmptyMessage : Message {
        public EmptyMessage(int id, MessageType type)
            : base(id, type, CodeRegistry.Empty) {

            if (IsConfirmable) {
                throw new ArgumentException("Confirmable message MUST NOT be empty.");
            }

            if (IsNonConfirmable) {
                throw new ArgumentException("Non-confirmable message MUST NOT be empty.");
            }
        }
    }
}