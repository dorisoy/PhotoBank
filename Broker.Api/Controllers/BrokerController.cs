using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Authentication;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Broker.Api.Utils;
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
            var messageClientId = ClientIdBuilder.Build(HttpContext);
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
            var messageClientId = ClientIdBuilder.Build(HttpContext);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var getPhotosInputMessage = new GetPhotosInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
            };
            _logger.LogInformation("Broker. GetPhotos. Send input message: " + messageChainId.Value);
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, getPhotosInputMessage);

            return Ok();
        }

        [HttpGet]
        [Route("getPhoto")]
        [CheckAuthentication]
        public IActionResult GetPhoto(string login, string token, int photoId)
        {
            var messageClientId = ClientIdBuilder.Build(HttpContext);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var getPhotoInputMessage = new GetPhotoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = photoId
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
            var messageClientId = ClientIdBuilder.Build(HttpContext);
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
    }
}
