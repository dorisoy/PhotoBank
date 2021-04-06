using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetPhotoAlbumsInputMessage))]
    public class GetPhotoAlbumsInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetPhotoAlbumsInputMessage>();
            var repo = _context.RepositoryFactory.Get<IPhotoAlbumRepository>();
            var albums = repo.GetPhotoAlbums(inputMessage.PhotoId, inputMessage.UserId);
            var albumsId = albums.Select(a => a.AlbumId).ToList();
            var outputMessage = new GetPhotoAlbumsOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                AlbumsId = albumsId
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
