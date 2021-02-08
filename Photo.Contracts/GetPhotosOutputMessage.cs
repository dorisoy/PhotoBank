using System;
using System.Collections.Generic;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class GetPhotosOutputMessage : OutputMessage
    {
        public GetPhotosOutputMessage(MessageClientId clientId, MessageChainId chainId, OutputMessageResult result) : base(clientId, chainId, result)
        {
        }

        public IEnumerable<int> PhotoIds { get; set; }
    }
}
