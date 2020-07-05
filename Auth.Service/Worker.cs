using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
using PhotoBank.DataAccess;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Auth.Service
{
    public class Worker : BackgroundService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IQueueManager _queueManager;
        private readonly ILogger<Worker> _logger;

        public Worker(IRepositoryFactory repositoryFactory, IQueueManager queueManager, ILogger<Worker> logger)
        {
            _repositoryFactory = repositoryFactory;
            _queueManager = queueManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _queueManager.Wait(AuthSettings.AuthInputQueue);
                if (message is CreateUserInputMessage)
                {
                    await Task.Factory.StartNew(() => ProcessCreateUserInputMessage((CreateUserInputMessage)message));
                }
            }
        }

        private void ProcessCreateUserInputMessage(CreateUserInputMessage inputMessage)
        {
            var user = new UserPoco
            {
                Login = inputMessage.Login,
                Name = inputMessage.Name,
                EMail = inputMessage.EMail
            };
            _repositoryFactory.Get<IUserRepository>().AddUser(user);
            var outputMessage = new CreateUserOutputMessage(inputMessage.Guid, OutputMessageResult.Success);
            _queueManager.Send(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
