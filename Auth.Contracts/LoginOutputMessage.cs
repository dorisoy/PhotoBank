using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class LoginOutputMessage : OutputMessage
    {
        public LoginOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public string Login { get; set; }

        public string Token { get; set; }

        public int UserId { get; set; }
    }
}
