using PhotoBank.DataAccess;
using PhotoBank.Logger.Common;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Service.Common.MessageProcessors
{
    public class MessageProcessorContext
    {
        public IQueueManager QueueManager { get; set; }

        public IRepositoryFactory RepositoryFactory { get; set; }

        public IMessageLogger Logger { get; set; }
    }
}
