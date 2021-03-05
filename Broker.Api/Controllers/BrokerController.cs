using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Authentication;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [EnableCors]
    public class BrokerController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IQueueManager _queueManager;
        private readonly ILogger<BrokerController> _logger;

        public BrokerController(IAuthenticationManager authenticationManager, IQueueManager queueManager, ILogger<BrokerController> logger)
        {
            _authenticationManager = authenticationManager;
            _queueManager = queueManager;
            _logger = logger;
            //_queueManager.Logger = _logger;
        }

        [HttpGet]
        [Route("testAction")]
        public IActionResult TestAction()
        {
            return new JsonResult(new { ok = true });
        }

        [HttpPost]
        [Route("createUser")]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new CreateUserInputMessage(messageClientId, messageChainId)
            {
                Login = request.Login,
                Password = request.Password,
                Name = request.Name,
                EMail = request.EMail
            };
            _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new LoginInputMessage(messageClientId, messageChainId)
            {
                Login = request.Login,
                Password = request.Password
            };
            _logger.LogInformation("Broker. Login. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getPhotos")]
        [CheckAuthentication]
        public IActionResult GetPhotos(GetPhotosRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var getPhotosInputMessage = new GetPhotosInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
            };
            _logger.LogInformation("Broker. GetPhotos. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, getPhotosInputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getPhoto")]
        [CheckAuthentication]
        public IActionResult GetPhoto(GetPhotoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var getPhotoInputMessage = new GetPhotoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId
            };
            _logger.LogInformation("Broker. GetPhoto. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, getPhotoInputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("uploadPhotos")]
        [CheckAuthentication]
        public IActionResult UploadPhotos(UploadPhotosRequest request)
        {
            if ((request.Files ?? Enumerable.Empty<string>()).Any() == false)
            {
                return BadRequest();
            }
            var messageClientId = new MessageClientId(request.ClientId);
            var userId = _authenticationManager.GetUserId(request.Login, request.Token);
            foreach (var fileBase64Content in request.Files)
            {
                var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
                var uploadPhotoInputMessage = new UploadPhotoInputMessage(messageClientId, messageChainId)
                {
                    UserId = userId,
                    FileBase64Content = fileBase64Content
                };
                _logger.LogInformation("Broker. UploadPhotos. Send input message: " + messageChainId.Value);
                _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, uploadPhotoInputMessage);
            }

            return Ok();
        }

        [HttpPost]
        [Route("deletePhoto")]
        [CheckAuthentication]
        public IActionResult DeletePhoto(DeletePhotoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var deletePhotoInputMessage = new DeletePhotoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId
            };
            _logger.LogInformation("Broker. DeletePhoto. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, deletePhotoInputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getPhotoAdditionalInfo")]
        [CheckAuthentication]
        public IActionResult GetPhotoAdditionalInfo(GetPhotoAdditionalInfoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var getPhotoAdditionalInfoInputMessage = new GetPhotoAdditionalInfoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId
            };
            _logger.LogInformation("Broker. GetPhotoAdditionalInfo. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, getPhotoAdditionalInfoInputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("setPhotoAdditionalInfo")]
        [CheckAuthentication]
        public IActionResult SetPhotoAdditionalInfo(SetPhotoAdditionalInfoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var setPhotoAdditionalInfoInputMessage = new SetPhotoAdditionalInfoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId,
                AdditionalInfo = request.AdditionalInfo
            };
            _logger.LogInformation("Broker. SetPhotoAdditionalInfo. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, setPhotoAdditionalInfoInputMessage);

            return Ok();
        }
    }
}
