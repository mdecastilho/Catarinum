namespace Catarinum {
    public class Request : Message {
        public Request(int id, MessageType type, MessageCode code)
            : base(id, type, code) {
        }
    }
}