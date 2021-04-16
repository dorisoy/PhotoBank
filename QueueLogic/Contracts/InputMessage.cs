using System;

namespace PhotoBank.QueueLogic.Contracts
{
    [Serializable]
    public abstract class InputMessage : Message
    {
        public InputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }
    }
}
