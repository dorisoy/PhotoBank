using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class LoginOutputMessage : OutputMessage
    {
        public LoginOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }

        public int UserId { get; set; }
    }
}
