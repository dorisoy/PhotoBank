using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoAlbumsInputMessage : InputMessage
    {
        public GetPhotoAlbumsInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int UserId { get; set; }

        public int PhotoId { get; set; }
    }
}
