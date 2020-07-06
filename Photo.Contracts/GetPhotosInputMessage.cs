using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotosInputMessage : InputMessage
    {
        public GetPhotosInputMessage(string guid) : base(guid)
        {
        }

        public int UserId { get; set; }
    }
}
