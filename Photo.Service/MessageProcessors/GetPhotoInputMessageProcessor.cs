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
            var inputMessage = GetMessageAs<GetPhotoInputMessage>();
            GetPhotoOutputMessage outputMessage;
            var photo = _context.RepositoryFactory.Get<IPhotoRepository>().GetPhoto(inputMessage.PhotoId);
            if (photo != null && File.Exists(photo.Path))
            {
                var photoBytes = File.ReadAllBytes(photo.Path);
                outputMessage = new GetPhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success) { PhotoBytes = photoBytes };
            }
            else
            {
                outputMessage = new GetPhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
            }
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
