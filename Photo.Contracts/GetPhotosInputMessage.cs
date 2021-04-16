using System;
using System.Collections.Generic;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotosInputMessage : InputMessage
    {
        public GetPhotosInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int UserId { get; set; }

        public IEnumerable<int> AlbumsId { get; set; }
    }
}
