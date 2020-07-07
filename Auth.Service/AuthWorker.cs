using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service
{
    public class AuthWorker : BackgroundService
    {
        private readonly IQueueManager _queueManager;
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly ILogger<AuthWorker> _logger;

        public AuthWorker(IQueueManager queueManager, IMessageProcessorFactory processorFactory, ILogger<AuthWorker> logger)
        {
            _queueManager = queueManager;
            _processorFactory = processorFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _queueManager.Wait(AuthSettings.AuthInputQueue);
                var processor = _processorFactory.MakeProcessorFor(message);
                await Task.Factory.StartNew(processor.Execute);
            }
        }
    }
}
