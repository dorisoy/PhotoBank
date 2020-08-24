using System.IO;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetPhotoInputMessage))]
    public class GetPhotoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var repo = _context.RepositoryFactory.Get<IPhotoRepository>();
            lock (repo)
            {
                var inputMessage = GetMessageAs<GetPhotoInputMessage>();
                GetPhotoOutputMessage outputMessage;
                var photo = repo.GetPhoto(inputMessage.PhotoId);
                if (photo != null && File.Exists(photo.Path))
                {
                    var photoBytes = File.ReadAllBytes(photo.Path);
                    outputMessage = new GetPhotoOutputMessage(inputMessage.Guid, OutputMessageResult.Success) { PhotoBytes = photoBytes };
                }
                else
                {
                    outputMessage = new GetPhotoOutputMessage(inputMessage.Guid, OutputMessageResult.Error);
                }
                _context.QueueManager.Send(PhotoSettings.PhotoOutputQueue, outputMessage);
            }
        }
    }
}
