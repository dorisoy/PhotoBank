using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class LoadUserPictureOutputMessage : OutputMessage
    {
        public LoadUserPictureOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public string PictureBase64Content { get; set; }

        public string NewPictureId { get; set; }
    }
}
