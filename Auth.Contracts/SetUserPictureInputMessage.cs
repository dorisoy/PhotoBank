using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class SetUserPictureInputMessage : InputMessage
    {
        public SetUserPictureInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public  int UserId { get; set; }

        public string NewPictureId { get; set; }
    }
}
