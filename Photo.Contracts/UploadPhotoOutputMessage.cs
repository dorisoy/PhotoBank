using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class UploadPhotoOutputMessage : OutputMessage
    {
        public UploadPhotoOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }

        public int PhotoId { get; set; }
    }
}
