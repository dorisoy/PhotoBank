using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrokerController : ControllerBase
    {
        private readonly IQueueManager _queueManager;
        private readonly ILogger<BrokerController> _logger;

        public BrokerController(IQueueManager queueManager, ILogger<BrokerController> logger)
        {
            _queueManager = queueManager;
            _logger = logger;
        }

        [HttpGet]
        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var inputMessage = new CreateUserInputMessage(inputMessageGuid)
            {
                Login = request.Login,
                Name = request.Name,
                EMail = request.EMail
            };
            _queueManager.Send(AuthSettings.AuthInputQueue, inputMessage);
            var outputMessage = _queueManager.WaitFor<CreateUserOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            if (outputMessage.Result == OutputMessageResult.Success)
            {
                return new CreateUserResponse { Success = true };
            }
            else
            {
                return new CreateUserResponse { Success = false };
            }
        }
    }
}
