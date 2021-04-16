using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class DeleteUserAlbumsInputMessage : InputMessage
    {
        public DeleteUserAlbumsInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int UserId { get; set; }

        public IEnumerable<int> AlbumsId { get; set; }
    }
}
