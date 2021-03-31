using System;
using System.IO;
using PhotoBank.Auth.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(LoadUserPictureInputMessage))]
    public class LoadUserPictureInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<LoadUserPictureInputMessage>();
            var filePath = Path.Combine(AuthSettings.UserPicturePath, inputMessage.ChainId.Value);
            var fileBytes = Convert.FromBase64String(inputMessage.PictureBase64Content);
            File.WriteAllBytes(filePath, fileBytes);
            var outputMessage = new LoadUserPictureOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                PictureBase64Content = inputMessage.PictureBase64Content,
                NewPictureId = inputMessage.ChainId.Value
            };
            _context.QueueManager.SendMessage(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
