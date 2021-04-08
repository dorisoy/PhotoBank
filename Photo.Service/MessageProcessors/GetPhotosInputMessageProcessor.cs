using System.Collections.Generic;
using System.Linq;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetPhotosInputMessage))]
    public class GetPhotosInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetPhotosInputMessage>();
            List<int> photosId = null;
            var albumsId = inputMessage.AlbumsId ?? Enumerable.Empty<int>();
            if (albumsId.Any())
            {
                var photoAlbums = _context.RepositoryFactory.Get<IPhotoAlbumRepository>().GetAlbumPhotos(albumsId, inputMessage.UserId);
                photosId = photoAlbums.Select(x => x.PhotoId).Distinct().ToList();
            }
            else
            {
                var photos = _context.RepositoryFactory.Get<IPhotoRepository>().GetUserPhotos(inputMessage.UserId);
                photosId = photos.Select(x => x.Id).ToList();
            }
            var outputMessage = new GetPhotosOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                PhotoIds = photosId
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
