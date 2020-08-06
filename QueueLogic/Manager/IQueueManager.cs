using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueManager
    {
        void Send(string queueName, Message messsage);

        Message Wait(string queueName);

        TMessage WaitFor<TMessage>(string queueName, string messageGuid) where TMessage : Message;

        IQueueListener CreateListener(string queueName);
    }
}
