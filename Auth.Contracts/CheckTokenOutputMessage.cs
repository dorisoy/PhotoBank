using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class CheckTokenOutputMessage : OutputMessage
    {
        public CheckTokenOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }

        public int UserId { get; set; }
    }
}
