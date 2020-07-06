using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class CreateUserOutputMessage : OutputMessage
    {
        public CreateUserOutputMessage(string guid, OutputMessageResult result) : base(guid, result)
        {
        }
    }
}
