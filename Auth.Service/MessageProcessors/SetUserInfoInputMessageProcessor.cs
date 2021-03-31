using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(SetUserInfoInputMessage))]
    public class SetUserInfoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<SetUserInfoInputMessage>();
            var user = _context.RepositoryFactory.Get<IUserRepository>().GetUser(inputMessage.Login);
            if (user != null)
            {
                user.Name = inputMessage.Name;
                user.EMail = inputMessage.EMail;
                user.About = inputMessage.About;
                _context.RepositoryFactory.Get<IUserRepository>().UpdateUser(user);
                var outputMessage = new SetUserInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success);
                _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
            }
            else
            {
                var outputMessage = new GetUserInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
                _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
            }
        }
    }
}
