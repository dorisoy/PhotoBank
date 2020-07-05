using System;

namespace PhotoBank.QueueLogic.Contracts
{
    public abstract class InputMessage : Message
    {
        public InputMessage(string guid) : base(guid)
        {
        }
    }
}
