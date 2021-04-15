using System;
using System.Text.Json;
using PhotoBank.Logger.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Logger.Common
{
    public class MessageLogger : IMessageLogger
    {
        private IQueueManager _queueManager;
        private readonly string _host;

        public MessageLogger(IQueueManager queueManager, string host)
        {
            _queueManager = queueManager;
            _host = host;
        }

        public void Begin(Message message)
        {
            var text = String.Format("Начало обработки сообщения. {0}. {1}", message.GetType().Name, JsonSerializer.Serialize(message));
            var logMessage = WriteLogInputMessage.MakeError(message, _host, text);
            Send(logMessage);
        }

        public void End(Message message)
        {
            var logMessage = WriteLogInputMessage.MakeError(message, _host, "Конец обработки сообщения.");
            Send(logMessage);
        }

        public void Info(MessageClientId messageClientId, MessageChainId messageChainId, string text)
        {
            var logMessage = WriteLogInputMessage.MakeInfo(messageClientId, messageChainId, _host, text);
            Send(logMessage);
        }

        public void Info(Message message, string text)
        {
            var logMessage = WriteLogInputMessage.MakeInfo(message, _host, text);
            Send(logMessage);
        }

        public void Warning(MessageClientId messageClientId, MessageChainId messageChainId, string text)
        {
            var logMessage = WriteLogInputMessage.MakeWarning(messageClientId, messageChainId, _host, text);
            Send(logMessage);
        }

        public void Warning(Message message, string text)
        {
            var logMessage = WriteLogInputMessage.MakeWarning(message, _host, text);
            Send(logMessage);
        }

        public void Error(Message message, Exception exp)
        {
            var logMessage = WriteLogInputMessage.MakeError(message, _host, exp.ToString());
            Send(logMessage);
        }

        public void InputMessageCreated(InputMessage inputMessage)
        {
            var text = String.Format("Создано входное сообщение {0}.", inputMessage.GetType().Name);
            var logMessage = WriteLogInputMessage.MakeInfo(inputMessage, _host, text);
            Send(logMessage);
        }

        private void Send(WriteLogInputMessage logMessage)
        {
            _queueManager.SendMessage(LoggerSettings.LoggerInputQueue, logMessage);
        }
    }
}
