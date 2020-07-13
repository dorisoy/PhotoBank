using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service
{
    public class PhotoWorker : BackgroundService
    {
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly IQueueManager _queueManager;
        private readonly ILogger<PhotoWorker> _logger;

        public PhotoWorker(IMessageProcessorFactory processorFactory, IQueueManager queueManager, ILogger<PhotoWorker> logger)
        {
            _processorFactory = processorFactory;
            _queueManager = queueManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _queueManager.Wait(PhotoSettings.PhotoInputQueue);
                var processor = _processorFactory.MakeProcessorFor(message);
                await Task.Factory.StartNew(processor.Execute);
            }
        }
    }
}