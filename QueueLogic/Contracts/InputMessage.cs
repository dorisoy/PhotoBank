using System;

namespace PhotoBank.QueueLogic.Contracts
{
    [Serializable]
    public abstract class InputMessage : Message
    {
        public InputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }
    }
}
