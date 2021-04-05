using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(DeleteUserAlbumsInputMessage))]
    public class DeleteUserAlbumsInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<DeleteUserAlbumsInputMessage>();
            var repo = _context.RepositoryFactory.Get<IAlbumRepository>();
            repo.DeleteAlbums(inputMessage.AlbumsId, inputMessage.UserId);
            var outputMessage = new DeleteUserAlbumsOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success);
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
