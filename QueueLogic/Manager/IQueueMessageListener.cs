using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueMessageListener<TMessage> : IDisposable where TMessage : Message
    {
        TMessage WaitForMessage();
    }
}
