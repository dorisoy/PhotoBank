using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.QueueLogic.Contracts
{
    [Serializable]
    public abstract class Message
    {
        public MessageClientId ClientId { get; private set; }

        public MessageChainId ChainId { get; private set; }

        public Message(MessageClientId clientId, MessageChainId chainId)
        {
            if (String.IsNullOrWhiteSpace(clientId.Value)) throw new ArgumentException("clientId");
            if (String.IsNullOrWhiteSpace(chainId.Value)) throw new ArgumentException("chainId");
            ClientId = clientId;
            ChainId = chainId;
        }
    }

    [Serializable]
    public class MessageClientId
    {
        public string Value { get; private set; }

        public MessageClientId(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is MessageClientId id &&
                   Value == id.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }

    [Serializable]
    public class MessageChainId
    {
        public string Value { get; private set; }

        public MessageChainId(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is MessageChainId id &&
                   Value == id.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
