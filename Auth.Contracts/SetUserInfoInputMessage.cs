using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class SetUserInfoInputMessage : InputMessage
    {
        public SetUserInfoInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public string Login { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        public string About { get; set; }
    }
}
