using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoAdditionalInfoInputMessage : InputMessage
    {
        public GetPhotoAdditionalInfoInputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }

        public int PhotoId { get; set; }
    }
}
