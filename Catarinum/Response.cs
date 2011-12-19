namespace Catarinum {
    public class Response : Message {
        public Response(int id, MessageType type, CodeRegistry code = CodeRegistry.Empty)
            : base(id, type, code) {
        }
    }
}