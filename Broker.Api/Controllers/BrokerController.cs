using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class BrokerController : ControllerBase
    {
        private readonly IQueueManager _queueManager;
        private readonly ILogger<BrokerController> _logger;

        public BrokerController(IQueueManager queueManager, ILogger<BrokerController> logger)
        {
            _queueManager = queueManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("createUser")]
        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var inputMessage = new CreateUserInputMessage(inputMessageGuid)
            {
                Login = request.Login,
                Password = request.Password,
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

        [HttpPost]
        [Route("login")]
        public LoginResponse Login(LoginRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var inputMessage = new LoginInputMessage(inputMessageGuid)
            {
                Login = request.Login,
                Password = request.Password
            };
            _queueManager.Send(AuthSettings.AuthInputQueue, inputMessage);
            var outputMessage = _queueManager.WaitFor<LoginOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            if (outputMessage.Result == OutputMessageResult.Success)
            {
                return new LoginResponse { Success = true };
            }
            else
            {
                return new LoginResponse { Success = false };
            }
        }

        [HttpPost]
        [Route("getPhotos")]
        public GetPhotosResponse GetPhotos(GetPhotosRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var loginInputMessage = new LoginInputMessage(inputMessageGuid)
            {
                Login = request.Login,
                Password = request.Password
            };
            _queueManager.Send(AuthSettings.AuthInputQueue, loginInputMessage);
            var loginOutputMessage = _queueManager.WaitFor<LoginOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            if (loginOutputMessage.Result == OutputMessageResult.Error)
            {
                return new GetPhotosResponse { Success = false };
            }
            var getPhotosInputMessage = new GetPhotosInputMessage(inputMessageGuid)
            {
                UserId = loginOutputMessage.UserId
            };
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotosInputMessage);
            var getPhotosOutputMessage = _queueManager.WaitFor<GetPhotosOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            if (getPhotosOutputMessage.Result == OutputMessageResult.Success)
            {
                return new GetPhotosResponse { Success = true, PhotoIds = getPhotosOutputMessage.PhotoIds };
            }
            else
            {
                return new GetPhotosResponse { Success = false };
            }
        }
    }
}
