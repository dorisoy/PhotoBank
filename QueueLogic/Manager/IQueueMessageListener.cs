using System;
using PhotoBank.QueueLogic;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueMessageListener<TMessage> where TMessage : Message
    {
        TMessage WaitForMessage();
    }
}
