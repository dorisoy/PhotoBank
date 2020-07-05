using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.QueueLogic.Contracts
{
    public abstract class OutputMessage : Message
    {
        public OutputMessageResult Result { get; private set; }

        public OutputMessage(string guid, OutputMessageResult result) : base(guid)
        {
            Result = result;
        }
    }

    public enum OutputMessageResult
    {
        Success,
        Error
    }
}
