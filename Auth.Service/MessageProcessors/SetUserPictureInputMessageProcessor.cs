using System.IO;
using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(SetUserPictureInputMessage))]
    public class SetUserPictureInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<SetUserPictureInputMessage>();
            var user = _context.RepositoryFactory.Get<IUserRepository>().GetUser(inputMessage.UserId);
            if (user != null)
            {
                // удаляем с диска старуют картинку
                File.Delete(user.Picture);
                // меняем на новую
                var pictureFilePath = Path.Combine(AuthSettings.UserPicturePath, inputMessage.NewPictureId);
                user.Picture = pictureFilePath;
                _context.RepositoryFactory.Get<IUserRepository>().UpdateUser(user);
                var outputMessage = new SetUserPictureOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success);
                _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
            }
            else
            {
                var outputMessage = new SetUserPictureOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
                _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
            }
        }
    }
}
