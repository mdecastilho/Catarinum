namespace Catarinum.Coap {
    public interface ITransaction {
        Message Message { get; set; }
        int Retransmissions { get; set; }
        int Timeout { get; set; }
        void ScheduleRetransmission();
        void Cancel();
    }
}