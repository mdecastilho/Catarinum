using System;

namespace Catarinum.Coap {
    public interface ITransaction {
        Message Message { get; set; }
        int Retransmissions { get; set; }
        int Timeout { get; set; }
        void ScheduleRetransmission(Action<ITransaction> callback, Action<ITransaction> error);
        void Cancel();
    }
}