using System;
using System.IO;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(UploadPhotoInputMessage))]
    public class UploadPhotoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<UploadPhotoInputMessage>();
            var filePath = Path.Combine(PhotoSettings.PhotoDatabasePath, inputMessage.Guid);
            var fileBytes = Convert.FromBase64String(inputMessage.FileBase64Content);
            File.WriteAllBytes(filePath, fileBytes);
            var photoId = _context.RepositoryFactory.Get<IPhotoRepository>().SavePhoto(inputMessage.UserId, filePath);
            var outputMessage = new UploadPhotoOutputMessage(inputMessage.Guid, OutputMessageResult.Success)
            {
                PhotoId = photoId
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
