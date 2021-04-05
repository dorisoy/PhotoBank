using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetUserAlbumsOutputMessage : OutputMessage
    {
        public GetUserAlbumsOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public IEnumerable<Album> Albums { get; set; }
    }
}
