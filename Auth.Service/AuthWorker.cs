using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PhotoBank.Auth.Contracts;
using PhotoBank.Logger.Common;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service
{
    public class AuthWorker : BackgroundService
    {
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly IMessageLogger _logger;

        public AuthWorker(
            IMessageProcessorFactory processorFactory, IQueueManager queueManager, IMessageLogger logger)
        {
            _processorFactory = processorFactory;
            _logger = logger;
            queueManager.AddMessageConsumer(AuthSettings.AuthInputQueue, OnMessageConsume);
        }

        private void OnMessageConsume(Message message)
        {
            try
            {
                _logger.Begin(message);
                var processor = _processorFactory.MakeProcessorFor(message);
                processor.Execute();
            }
            catch (Exception exp)
            {
                _logger.Error(message, exp);
            }
            finally
            {
                _logger.End(message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
