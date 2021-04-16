using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class UploadPhotoInputMessage : InputMessage
    {
        public UploadPhotoInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int UserId { get; set; }

        public string FileBase64Content { get; set; }
    }
}
