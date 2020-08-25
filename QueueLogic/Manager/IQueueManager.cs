using PhotoBank.QueueLogic;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueManager
    {
        void Send(string queueName, Message messsage);

        IQueueListener CreateQueueListener(string queueName);

        IQueueMessageListener<TMessage> CreateQueueMessageListener<TMessage>(string queueName, string messageGuid) where TMessage : Message;
    }
}
