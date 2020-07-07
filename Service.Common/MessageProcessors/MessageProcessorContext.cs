using PhotoBank.DataAccess;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Service.Common.MessageProcessors
{
    public class MessageProcessorContext
    {
        public IQueueManager QueueManager { get; set; }

        public IRepositoryFactory RepositoryFactory { get; set; }
    }
}
