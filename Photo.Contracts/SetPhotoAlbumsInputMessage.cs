using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class SetPhotoAlbumsInputMessage : InputMessage
    {
        public SetPhotoAlbumsInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int UserId { get; set; }

        public int PhotoId { get; set; }

        public IEnumerable<int> AlbumsId { get; set; }

        public IEnumerable<string> AlbumsName { get; set; }
    }
}
