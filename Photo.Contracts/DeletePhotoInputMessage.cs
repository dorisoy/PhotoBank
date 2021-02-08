using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class DeletePhotoInputMessage : InputMessage
    {
        public DeletePhotoInputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }

        public int PhotoId { get; set; }
    }
}
