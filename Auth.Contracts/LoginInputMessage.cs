using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Auth.Contracts
{
    [Serializable]
    public class LoginInputMessage : InputMessage
    {
        public LoginInputMessage(string guid) : base(guid)
        {
        }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
