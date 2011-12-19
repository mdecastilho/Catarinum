namespace Catarinum {
    public class Request : Message {
        public Request(int id, MessageType type, CodeRegistry code)
            : base(id, type, code) {
        }
    }
}