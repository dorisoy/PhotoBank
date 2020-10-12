using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class CheckTokenInputMessage : InputMessage
    {
        public CheckTokenInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public string Login { get; set; }

        public string Token { get; set; }
    }
}
