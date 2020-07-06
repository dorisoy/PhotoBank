using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotosOutputMessage : OutputMessage
    {
        public GetPhotosOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }
    }
}
