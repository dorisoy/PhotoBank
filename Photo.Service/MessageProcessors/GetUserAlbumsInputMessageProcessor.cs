using System.Linq;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetUserAlbumsInputMessage))]
    public class GetUserAlbumsInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetUserAlbumsInputMessage>();
            var albumsPoco = _context.RepositoryFactory.Get<IAlbumRepository>().GetUserAlbums(inputMessage.UserId);
            var albums = albumsPoco.Select(a => new Album { Id = a.Id, Name = a.Name }).ToList();
            var outputMessage = new GetUserAlbumsOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                Albums = albums
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
