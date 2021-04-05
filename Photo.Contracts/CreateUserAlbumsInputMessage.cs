using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class CreateUserAlbumsInputMessage : InputMessage
    {
        public CreateUserAlbumsInputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }

        public int UserId { get; set; }

        public IEnumerable<NewAlbum> NewAlbums { get; set; }
    }
}
