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
            var fileName = inputMessage.ChainId.Value;
            var filePath = Path.Combine(PhotoSettings.RootPhotoPath, fileName);
            var fileBytes = Convert.FromBase64String(inputMessage.FileBase64Content);
            File.WriteAllBytes(filePath, fileBytes);
            var photo = new PhotoPoco
            {
                UserId = inputMessage.UserId,
                Path = fileName,
                Description = "Фото загружено: " + DateTime.Now,
                CreateDate = DateTime.Now
            };
            var photoId = _context.RepositoryFactory.Get<IPhotoRepository>().SavePhoto(photo);
            var outputMessage = new UploadPhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                PhotoId = photoId
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
