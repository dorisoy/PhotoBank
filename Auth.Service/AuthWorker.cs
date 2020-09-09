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
        private readonly IMessageProcessorFactory _processorFactory;
        private readonly IQueueListener _queueListener;
        private readonly ILogger<AuthWorker> _logger;

        public AuthWorker(IMessageProcessorFactory processorFactory, IQueueManager queueManager, ILogger<AuthWorker> logger)
        {
            _processorFactory = processorFactory;
            _queueListener = queueManager.CreateQueueListener(AuthSettings.AuthInputQueue);
            _queueListener.ReceiveMessage += OnReceiveMessage;
            _logger = logger;
        }

        private void OnReceiveMessage(object sender, ReceiveMessageEventArgs e)
        {
            _logger.LogInformation("Get input message: " + e.Message.Guid);
            var processor = _processorFactory.MakeProcessorFor(e.Message);
            Task.Factory.StartNew(processor.Execute).ContinueWith(task => _logger.LogInformation("Send output message: " + e.Message.Guid));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _queueListener.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
