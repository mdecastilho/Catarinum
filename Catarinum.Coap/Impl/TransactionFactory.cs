namespace Catarinum.Coap.Impl {
    public class TransactionFactory : ITransactionFactory {
        public ITransaction Create(MessageLayer messageLayer, Message message) {
            return new Transaction(messageLayer, new RetransmissionTimer()) { Message = message };
        }
    }
}