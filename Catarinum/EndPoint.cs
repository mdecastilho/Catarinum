namespace Catarinum {
    public class EndPoint {
        private const int ResponseTimeout = 2000;
        private const double ResponseRandomFactor = 1.5;
        private const int MaxRetransmit = 4;

        public int Timeout { get; set; }
        public int RestransmissionCounter { get; set; }

        // The Message ID of the response MUST
        // match that of the original message.  For unicast messages, the source
        // of the response MUST match the destination of the original message.

        //  a new confirmable message, the initial timeout is set
        // to a random number between RESPONSE_TIMEOUT and (RESPONSE_TIMEOUT *
        // RESPONSE_RANDOM_FACTOR), and the retransmission counter is set to 0.
        // When the timeout is triggered and the retransmission counter is less
        // than MAX_RETRANSMIT, the message is retransmitted, the retransmission
        // counter is incremented, and the timeout is doubled.  If the
        // retransmission counter reaches MAX_RETRANSMIT on a timeout, or if the
        // end-point receives a reset message, then the attempt to transmit the
        // message is canceled and the application process informed of failure.

        // The same Message ID MUST NOT be re-used (per Message
        // ID variable) within the potential retransmission window, calculated
        // as RESPONSE_TIMEOUT * RESPONSE_RANDOM_FACTOR * (2 ^ MAX_RETRANSMIT -
        // 1) plus the expected maximum round trip time.
    }
}