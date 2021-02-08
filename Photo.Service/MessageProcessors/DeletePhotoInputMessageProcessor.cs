using System.IO;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(DeletePhotoInputMessage))]
    public class DeletePhotoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<DeletePhotoInputMessage>();
            var photo = _context.RepositoryFactory.Get<IPhotoRepository>().GetPhoto(inputMessage.PhotoId);
            if (photo != null)
            {
                _context.RepositoryFactory.Get<IPhotoRepository>().DeletePhoto(photo);
                File.Delete(photo.Path);
            }
            var outputMessage = new DeletePhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                PhotoId = inputMessage.PhotoId
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
