using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoInputMessage : InputMessage
    {
        public GetPhotoInputMessage(string guid) : base(guid)
        {
        }

        public int PhotoId { get; set; }
    }
}
