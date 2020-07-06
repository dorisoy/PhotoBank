using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotoOutputMessage : OutputMessage
    {
        public GetPhotoOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }

        public byte[] PhotoBytes { get; set; }
    }
}
