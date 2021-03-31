using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class SetUserPictureOutputMessage : OutputMessage
    {
        public SetUserPictureOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }
    }
}
