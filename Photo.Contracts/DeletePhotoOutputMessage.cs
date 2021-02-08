using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class DeletePhotoOutputMessage : OutputMessage
    {
        public DeletePhotoOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public int PhotoId { get; set; }
    }
}
