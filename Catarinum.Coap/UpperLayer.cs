namespace Catarinum.Coap {
    public abstract class UpperLayer : Layer {
        private ILayer _lowerLayer;

        public ILayer LowerLayer {
            get { return _lowerLayer; }
            private set {
                _lowerLayer = value;
                _lowerLayer.RegisterObserver(this);
            }
        }

        protected UpperLayer(ILayer lowerLayer) {
            LowerLayer = lowerLayer;
        }

        public void SendMessageOverLowerLayer(Message message) {
            LowerLayer.Send(message);
        }
    }
}