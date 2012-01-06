using Catarinum.Coap.Impl;

namespace Catarinum.Coap {
    public interface ITransactionFactory {
        ITransaction Create(MessageLayer messageLayer, Message message);
    }
}