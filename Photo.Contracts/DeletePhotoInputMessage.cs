using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class DeletePhotoInputMessage : InputMessage
    {
        public DeletePhotoInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int PhotoId { get; set; }
    }
}
