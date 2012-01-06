namespace Catarinum.Coap {
    public class MessageFactory {
        public Message Create(MessageType type, CodeRegistry code, int id) {
            Message message;

            if (Request.IsValidCodeRegistry(code)) {
                message = new Request(code, type == MessageType.Confirmable);
            }
            else if (Response.IsValidCodeRegistry(code)) {
                message = new Response(type, code);
            }
            else {
                message = new Message(type);
            }

            message.Id = id;
            return message;
        }
    }
}