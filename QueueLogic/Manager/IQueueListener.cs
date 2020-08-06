using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager
{
    public interface IQueueListener : IDisposable
    {
        Message WaitForMessage();
    }
}
