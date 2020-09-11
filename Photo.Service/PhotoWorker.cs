using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service
{
    public class PhotoWorker : BackgroundService
    {
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly ILogger<PhotoWorker> _logger;

        public PhotoWorker(IMessageProcessorFactory processorFactory, IQueueManager queueManager, ILogger<PhotoWorker> logger)
        {
            _processorFactory = processorFactory;
            _logger = logger;
            queueManager.AddMessageConsumer(PhotoSettings.PhotoInputQueue, OnMessageConsume);
        }

        private void OnMessageConsume(Message message)
        {
            _logger.LogInformation("Get input message: " + message.Guid);
            var processor = _processorFactory.MakeProcessorFor(message);
            processor.Execute();
            _logger.LogInformation("Send output message: " + message.Guid);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
