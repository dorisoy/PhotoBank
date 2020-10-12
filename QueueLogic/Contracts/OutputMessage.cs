using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.QueueLogic.Contracts
{
    [Serializable]
    public abstract class OutputMessage : Message
    {
        public OutputMessageResult Result { get; private set; }

        public OutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId)
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
