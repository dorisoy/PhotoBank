using System;

namespace PhotoBank.QueueLogic.Contracts
{
    [Serializable]
    public abstract class InputMessage : Message
    {
        public InputMessage(string guid) : base(guid)
        {
        }
    }
}
