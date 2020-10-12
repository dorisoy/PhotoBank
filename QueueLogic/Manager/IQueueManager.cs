using Microsoft.Extensions.Logging;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueManager
    {
        void SendMessage(string queueName, Message message);

        void AddMessageConsumer(string queueName, MessageConsumerCallback callback);

        ILogger Logger { get; set; }
    }

    public delegate void MessageConsumerCallback(Message message);
}
