namespace Catarinum.Coap.Layers {
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

        protected virtual void SendMessageOverLowerLayer(Message message) {
            LowerLayer.Send(message);
        }
    }
}