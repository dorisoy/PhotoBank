using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(SetPhotoAdditionalInfoInputMessage))]
    public class SetPhotoAdditionalInfoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<SetPhotoAdditionalInfoInputMessage>();
            SetPhotoAdditionalInfoOutputMessage outputMessage;
            var photo = _context.RepositoryFactory.Get<IPhotoRepository>().GetPhoto(inputMessage.PhotoId);
            if (photo != null)
            {
                bool needToSave = false;
                if (inputMessage.AdditionalInfo.Description != null)
                {
                    photo.Description = inputMessage.AdditionalInfo.Description.Substring(0, 500);
                    needToSave = true;
                }
                if (needToSave)
                {
                    _context.RepositoryFactory.Get<IPhotoRepository>().UpdatePhoto(photo);
                }
                outputMessage = new SetPhotoAdditionalInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
                {
                    PhotoId = inputMessage.PhotoId
                };
            }
            else
            {
                outputMessage = new SetPhotoAdditionalInfoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
            }
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
