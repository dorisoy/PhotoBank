using System;
using System.Collections.Generic;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotosInputMessage : InputMessage
    {
        public GetPhotosInputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }

        public int UserId { get; set; }

        public IEnumerable<int> AlbumsId { get; set; }
    }
}
