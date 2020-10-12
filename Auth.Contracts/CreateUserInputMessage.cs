using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class CreateUserInputMessage : InputMessage
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string EMail { get; set; }

        public CreateUserInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }
    }
}
