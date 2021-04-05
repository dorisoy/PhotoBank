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
    [MessageProcessor(MessageType = typeof(CreateUserAlbumsInputMessage))]
    public class CreateUserAlbumsInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<CreateUserAlbumsInputMessage>();
            var albumsPoco = inputMessage.NewAlbums.Select(a => new AlbumPoco { Name = a.Name, UserId = inputMessage.UserId }).ToList();
            var repo = _context.RepositoryFactory.Get<IAlbumRepository>();
            repo.SaveAlbums(albumsPoco);
            var albums = albumsPoco.Select(a => new Album { Id = a.Id, Name = a.Name }).ToList();
            var outputMessage = new CreateUserAlbumsOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
            {
                Albums = albums
            };
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
