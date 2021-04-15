using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.Logger.Contracts;
using PhotoBank.Logger.Service.Data;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Logger.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(WriteLogInputMessage))]
    public class WriteLogInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<WriteLogInputMessage>();
            var log = new LogPoco
            {
                ClientId = inputMessage.ClientId.Value,
                ChainId = inputMessage.ChainId.Value,
                Severity = (int)inputMessage.Severity,
                Host = inputMessage.Host,
                Text = inputMessage.Text,
                CreateDate = inputMessage.CreateDate
            };
            var repo = _context.RepositoryFactory.Get<ILogRepository>();
            repo.SaveLog(log);
        }
    }
}
