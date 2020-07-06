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
    public class AuthWorker : BackgroundService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IQueueManager _queueManager;
        private readonly ILogger<AuthWorker> _logger;

        public AuthWorker(IRepositoryFactory repositoryFactory, IQueueManager queueManager, ILogger<AuthWorker> logger)
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
                else if (message is LoginInputMessage)
                {
                    await Task.Factory.StartNew(() => ProcessLoginInputMessage((LoginInputMessage)message));
                }
            }
        }

        private void ProcessCreateUserInputMessage(CreateUserInputMessage inputMessage)
        {
            var user = new UserPoco
            {
                Login = inputMessage.Login,
                Password = inputMessage.Password,
                Name = inputMessage.Name,
                EMail = inputMessage.EMail
            };
            _repositoryFactory.Get<IUserRepository>().AddUser(user);
            var outputMessage = new CreateUserOutputMessage(inputMessage.Guid, OutputMessageResult.Success);
            _queueManager.Send(AuthSettings.AuthOutputQueue, outputMessage);
        }

        private void ProcessLoginInputMessage(LoginInputMessage inputMessage)
        {
            var user = _repositoryFactory.Get<IUserRepository>().GetUser(inputMessage.Login, inputMessage.Password);
            var messageResult = user != null ? OutputMessageResult.Success : OutputMessageResult.Error;
            var userId = user != null ? user.Id : 0;
            var outputMessage = new LoginOutputMessage(inputMessage.Guid, messageResult) { UserId = userId };
            _queueManager.Send(AuthSettings.AuthOutputQueue, outputMessage);
        }
    }
}
