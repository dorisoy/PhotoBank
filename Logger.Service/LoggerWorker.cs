using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Logger.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace Logger.Service
{
    public class LoggerWorker : BackgroundService
    {
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly ILogger<LoggerWorker> _logger;

        public LoggerWorker(IMessageProcessorFactory processorFactory, IQueueManager queueManager, ILogger<LoggerWorker> logger)
        {
            _processorFactory = processorFactory;
            _logger = logger;
            queueManager.AddMessageConsumer(LoggerSettings.LoggerInputQueue, OnMessageConsume);
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
