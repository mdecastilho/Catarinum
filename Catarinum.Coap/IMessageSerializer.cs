namespace Catarinum.Coap {
    public interface IMessageSerializer {
        byte[] Serialize(Message message);
        Message Deserialize(byte[] bytes);
    }
}