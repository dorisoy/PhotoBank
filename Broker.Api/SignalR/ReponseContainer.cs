using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Broker.Api.SignalR
{
    public class ReponseContainer
    {
        public MessageClientId MessageClientId { get; set; }

        public object Response { get; set; }
    }
}
