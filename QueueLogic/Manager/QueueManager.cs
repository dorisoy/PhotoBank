using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public class QueueManager : IQueueManager
    {
        public void Send(string queueName, Message messsage)
        {
            throw new NotImplementedException();
        }

        public Message Wait(string queueName)
        {
            throw new NotImplementedException();
        }

        public TMessage WaitFor<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            throw new NotImplementedException();
        }
    }
}
