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
    [MessageProcessor(MessageType = typeof(GetUserInfoInputMessage))]
    public class GetUserInfoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetUserInfoInputMessage>();
            var user = _context.RepositoryFactory.Get<IUserRepository>().GetUser(inputMessage.UserId);
            if (user != null)
            {
                var pictureFileContent = File.ReadAllBytes(user.Picture);
                var pictureFileBase64Content = Convert.ToBase64String(pictureFileContent);
                var outputMessage = new GetUserInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
                {
                    Name = user.Name,
                    EMail = user.EMail,
                    About = user.About,
                    PictureBase64Content = pictureFileBase64Content
                };
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
