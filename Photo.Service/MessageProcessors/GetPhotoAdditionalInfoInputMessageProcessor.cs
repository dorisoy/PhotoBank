using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetPhotoAdditionalInfoInputMessage))]
    public class GetPhotoAdditionalInfoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetPhotoAdditionalInfoInputMessage>();
            var additionalInfo = new PhotoAdditionalInfo();
            var photo = _context.RepositoryFactory.Get<IPhotoRepository>().GetPhoto(inputMessage.PhotoId);
            if (photo != null)
            {
                additionalInfo.Description = photo.Description;
            }
            var outputMessage = new GetPhotoAdditionalInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                PhotoId = inputMessage.PhotoId,
                AdditionalInfo = additionalInfo
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
