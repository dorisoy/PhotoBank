using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class SetPhotoAdditionalInfoInputMessage : InputMessage
    {
        public SetPhotoAdditionalInfoInputMessage(MessageClientId clientId, MessageChainId chainId) : base(clientId, chainId)
        {
        }

        public int PhotoId { get; set; }

        public PhotoAdditionalInfo AdditionalInfo { get; set; }
    }
}
