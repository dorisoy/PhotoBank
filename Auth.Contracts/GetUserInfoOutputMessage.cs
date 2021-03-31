using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class GetUserInfoOutputMessage : OutputMessage
    {
        public GetUserInfoOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public string Name { get; set; }

        public string EMail { get; set; }

        public string About { get; set; }

        public string PictureBase64Content { get; set; }
    }
}
