using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service
{
    public class AuthWorker : BackgroundService
    {
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly ILogger<AuthWorker> _logger;

        public AuthWorker(IMessageProcessorFactory processorFactory, IQueueManager queueManager, ILogger<AuthWorker> logger)
        {
            _processorFactory = processorFactory;
            _logger = logger;
            queueManager.AddMessageConsumer(AuthSettings.AuthInputQueue, OnMessageConsume);
        }

        private void OnMessageConsume(Message message)
        {
            _logger.LogInformation("Get input message: " + message.ChainId.Value);
            var processor = _processorFactory.MakeProcessorFor(message);
            processor.Execute();
            _logger.LogInformation("Send output message: " + message.ChainId.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
