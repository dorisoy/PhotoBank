using System;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Logger.Common
{
    public interface IMessageLogger
    {
        void Begin(Message message);

        void End(Message message);

        void Info(MessageClientId messageClientId, MessageChainId messageChainId, string text);

        void Info(Message message, string text);

        void Warning(Message message, string text);

        void Warning(MessageClientId messageClientId, MessageChainId messageChainId, string text);

        void Error(Message message, Exception exp);

        void InputMessageCreated(InputMessage inputMessage);
    }
}
