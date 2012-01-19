using System.Collections.Generic;

namespace Catarinum.Coap.Layers {
    public class MessageLayer : UpperLayer {
        public static int ResponseTimeout = 2000;
        public static double ResponseRandomFactor = 1.5;
        public static int MaxRetransmissions = 4;
        private readonly ITransmissionContextFactory _transmissionContextFactory;
        private readonly Dictionary<int, ITransmissionContext> _retransmissions;
        private readonly IdSequence _idSequence;
        private readonly MessageCache _replyCache;
        private readonly MessageCache _duplicationCache;

        public MessageLayer()
            : this(new TransportLayer()) {
        }

        public MessageLayer(ILayer lowerLayer)
            : this(lowerLayer, new TransmissionContextFactory()) {
        }

        public MessageLayer(ILayer lowerLayer, ITransmissionContextFactory transmissionContextFactory)
            : base(lowerLayer) {
            _transmissionContextFactory = transmissionContextFactory;
            _retransmissions = new Dictionary<int, ITransmissionContext>();
            _idSequence = new IdSequence();
            _replyCache = new MessageCache();
            _duplicationCache = new MessageCache();
        }

        public IEnumerable<ITransmissionContext> Retransmissions {
            get { return _retransmissions.Values; }
        }

        public override void Send(Message message) {
            if (message.Id == 0) {
                message.Id = _idSequence.NextId();
            }

            if (message.IsConfirmable) {
                var transaction = _transmissionContextFactory.Create(this, message);
                _retransmissions.Add(message.Id, transaction);
                transaction.ScheduleRetry(RetryCallback, ErrorCallback);
            }
            else if (message.IsReply) {
                _replyCache.Add(message);
            }

            SendMessageOverLowerLayer(message);
        }

        public override void OnReceive(Message message) {
            if (message.Id > 0) {
                if (_duplicationCache.ContainsMessage(message)) {
                    if (message.IsConfirmable) {
                        var reply = _replyCache.Get(message);
                        SendMessageOverLowerLayer(reply);
                    }

                    return;
                }

                _duplicationCache.Add(message);

                if (message.IsReply) {
                    if (_retransmissions.ContainsKey(message.Id)) {
                        var transaction = _retransmissions[message.Id];
                        transaction.Cancel();
                        _retransmissions.Remove(message.Id);
                    }
                }
            }

            base.OnReceive(message);
        }

        private void RetryCallback(ITransmissionContext transmissionContext) {
            SendMessageOverLowerLayer(transmissionContext.Message);
        }

        private void ErrorCallback(ITransmissionContext transmissionContext) {
            transmissionContext.Cancel();
            _retransmissions.Remove(transmissionContext.Message.Id);
        }
    }
}