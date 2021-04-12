using System.Linq;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(SetPhotoAlbumsInputMessage))]
    public class SetPhotoAlbumsInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<SetPhotoAlbumsInputMessage>();
            var photoAlbumsRepo = _context.RepositoryFactory.Get<IPhotoAlbumRepository>();
            photoAlbumsRepo.DeletePhotoAlbums(inputMessage.PhotoId, inputMessage.UserId);
            photoAlbumsRepo.AddPhotoAlbums(inputMessage.PhotoId, inputMessage.AlbumsId, inputMessage.UserId);

            var albumsRepo = _context.RepositoryFactory.Get<IAlbumRepository>();
            var albums = albumsRepo.GetUserAlbums(inputMessage.UserId).GroupBy(x=>x.Name).ToDictionary(k => k.Key, v => v.First());
            foreach (var albumName in inputMessage.AlbumsName)
            {
                if (albums.ContainsKey(albumName))
                {
                    var album = albums[albumName];
                    photoAlbumsRepo.AddPhotoAlbum(inputMessage.PhotoId, album.Id, album.UserId);
                }
                else
                {
                    var album = new AlbumPoco { Name = albumName, UserId = inputMessage.UserId };
                    albumsRepo.SaveAlbum(album);
                    photoAlbumsRepo.AddPhotoAlbum(inputMessage.PhotoId, album.Id, album.UserId);
                }
            }

            var outputMessage = new SetPhotoAlbumsOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success);
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
