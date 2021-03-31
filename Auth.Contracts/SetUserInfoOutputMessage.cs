using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class SetUserInfoOutputMessage : OutputMessage
    {
        public SetUserInfoOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }
    }
}
