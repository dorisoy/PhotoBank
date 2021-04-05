using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoAlbumsOutputMessage : OutputMessage
    {
        public GetPhotoAlbumsOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public IEnumerable<int> AlbumsId { get; set; }
    }
}
