using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.QueueLogic.Contracts
{
    public abstract class Message
    {
        public string Guid { get; private set; }

        public Message(string guid)
        {
            if (String.IsNullOrWhiteSpace(guid)) throw new ArgumentException("guid");
            Guid = guid;
        }
    }
}
