using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class SetPhotoAdditionalInfoOutputMessage : OutputMessage
    {
        public SetPhotoAdditionalInfoOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public int PhotoId { get; set; }
    }
}
