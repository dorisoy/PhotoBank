using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class UploadPhotoInputMessage : InputMessage
    {
        public UploadPhotoInputMessage(string guid) : base(guid)
        {
        }

        public int UserId { get; set; }

        public string FileBase64Content { get; set; }
    }
}
