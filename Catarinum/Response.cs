namespace Catarinum {
    public class Response : Message {
        public Response(int id, MessageType type, MessageCode code = MessageCode.Empty)
            : base(id, type, code) {
        }
    }
}