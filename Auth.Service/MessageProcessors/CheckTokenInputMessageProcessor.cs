using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(CheckTokenInputMessage))]
    public class CheckTokenInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<CheckTokenInputMessage>();
            var token = _context.RepositoryFactory.Get<ITokenRepository>().GetToken(inputMessage.Login, inputMessage.Token);
            var messageResult = token != null ? OutputMessageResult.Success : OutputMessageResult.Error;
            var userId = token != null ? token.UserId : 0;
            var outputMessage = new CheckTokenOutputMessage(inputMessage.Guid, messageResult) { UserId = userId };
            _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
