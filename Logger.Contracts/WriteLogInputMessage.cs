using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Logger.Contracts
{
    [Serializable]
    public class WriteLogInputMessage : InputMessage
    {
        public WriteLogInputMessage(MessageClientId userId, MessageChainId chainId) : base(userId, chainId)
        {
        }

        public string Host { get; set; }

        public string Text { get; set; }

        public Severity Severity { get; set; }

        public DateTime CreateDate { get; set; }

        public static WriteLogInputMessage MakeError(Message message, string host, string text)
        {
            return new WriteLogInputMessage(message.ClientId, message.ChainId)
            {
                Host = host,
                Text = text,
                Severity = Severity.Error,
                CreateDate = DateTime.Now
            };
        }

        public static WriteLogInputMessage MakeWarning(Message message, string host, string text)
        {
            return new WriteLogInputMessage(message.ClientId, message.ChainId)
            {
                Host = host,
                Text = text,
                Severity = Severity.Warning,
                CreateDate = DateTime.Now
            };
        }

        public static WriteLogInputMessage MakeInfo(MessageClientId messageClientId, MessageChainId messageChainId, string host, string text)
        {
            return new WriteLogInputMessage(messageClientId, messageChainId)
            {
                Host = host,
                Text = text,
                Severity = Severity.Warning,
                CreateDate = DateTime.Now
            };
        }

        public static WriteLogInputMessage MakeInfo(Message message, string host, string text)
        {
            return new WriteLogInputMessage(message.ClientId, message.ChainId)
            {
                Host = host,
                Text = text,
                Severity = Severity.Info,
                CreateDate = DateTime.Now
            };
        }

        public static WriteLogInputMessage MakeWarning(MessageClientId messageClientId, MessageChainId messageChainId, string host, string text)
        {
            return new WriteLogInputMessage(messageClientId, messageChainId)
            {
                Host = host,
                Text = text,
                Severity = Severity.Warning,
                CreateDate = DateTime.Now
            };
        }
    }
}
