using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoOutputMessage : OutputMessage
    {
        public GetPhotoOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public int PhotoId { get; set; }

        public string FileBase64Content { get; set; }
    }
}
