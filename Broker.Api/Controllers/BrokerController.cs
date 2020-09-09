using System;
using System.Collections.Generic;
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
    [EnableCors("TCAPolicy")]
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
            _queueManager.Logger = _logger;
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
            var createUserOutputMessageListener = _queueManager.CreateQueueMessageListener<CreateUserOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            var outputMessage = createUserOutputMessageListener.WaitForMessage();
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
            _logger.LogInformation("Broker. Login. Send input message: " + inputMessageGuid);
            _queueManager.Send(AuthSettings.AuthInputQueue, inputMessage);
            var loginOutputMessageListener = _queueManager.CreateQueueMessageListener<LoginOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            _logger.LogInformation("Broker. Login. Waiting for output message: " + inputMessageGuid);
            var outputMessage = loginOutputMessageListener.WaitForMessage();
            _logger.LogInformation("Broker. Login. Recieve output message: " + inputMessageGuid);
            if (outputMessage.Result == OutputMessageResult.Success)
            {
                _authenticationManager.Add(request.Login, outputMessage.Token, outputMessage.UserId);
                return new LoginResponse { Success = true, Token = outputMessage.Token };
            }
            else
            {
                return new LoginResponse { Success = false };
            }
        }

        [HttpPost]
        [Route("getPhotos")]
        [CheckAuthentication]
        public GetPhotosResponse GetPhotos(GetPhotosRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var getPhotosInputMessage = new GetPhotosInputMessage(inputMessageGuid)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
            };
            _logger.LogInformation("Broker. GetPhotos. Send input message: " + inputMessageGuid);
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotosInputMessage);
            var getPhotosOutputMessageListener = _queueManager.CreateQueueMessageListener<GetPhotosOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            _logger.LogInformation("Broker. GetPhotos. Waiting for output message: " + inputMessageGuid);
            var getPhotosOutputMessage = getPhotosOutputMessageListener.WaitForMessage();
            _logger.LogInformation("Broker. GetPhotos. Recieve output message: " + inputMessageGuid);
            if (getPhotosOutputMessage.Result == OutputMessageResult.Success)
            {
                return new GetPhotosResponse { Success = true, PhotoIds = getPhotosOutputMessage.PhotoIds };
            }
            else
            {
                return new GetPhotosResponse { Success = false };
            }
        }

        [HttpGet]
        [Route("getPhoto")]
        [CheckAuthentication]
        public IActionResult GetPhoto(string login, string token, int photoId)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var getPhotoInputMessage = new GetPhotoInputMessage(inputMessageGuid)
            {
                PhotoId = photoId
            };
            _logger.LogInformation("Broker. GetPhoto. Send input message: " + inputMessageGuid);
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotoInputMessage);
            var getPhotoOutputMessageListener = _queueManager.CreateQueueMessageListener<GetPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
            _logger.LogInformation("Broker. GetPhoto. Waiting for output message: " + inputMessageGuid);
            var getPhotoOutputMessage = getPhotoOutputMessageListener.WaitForMessage();
            _logger.LogInformation("Broker. GetPhoto. Recieve output message: " + inputMessageGuid);
            if (getPhotoOutputMessage.Result == OutputMessageResult.Success)
            {
                return File(getPhotoOutputMessage.PhotoBytes, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("uploadPhotos")]
        [CheckAuthentication]
        public UploadPhotosReponse UploadPhotos(UploadPhotosRequest request)
        {
            if ((request.Files ?? Enumerable.Empty<string>()).Any() == false)
            {
                return new UploadPhotosReponse { Success = false };
            }
            var inputMessageGuidList = new List<string>();
            var uploadedPhotoIds = new List<int>();
            foreach (var fileBase64Content in request.Files)
            {
                var inputMessageGuid = Guid.NewGuid().ToString();
                inputMessageGuidList.Add(inputMessageGuid);
                var uploadPhotoInputMessage = new UploadPhotoInputMessage(inputMessageGuid)
                {
                    UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                    FileBase64Content = fileBase64Content
                };
                _logger.LogInformation("Broker. UploadPhotos. Send input message: " + inputMessageGuid);
                _queueManager.Send(PhotoSettings.PhotoInputQueue, uploadPhotoInputMessage);
            }
            foreach (var inputMessageGuid in inputMessageGuidList)
            {
                var uploadPhotoOutputMessageListener = _queueManager.CreateQueueMessageListener<UploadPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
                _logger.LogInformation("Broker. UploadPhotos. Waiting for output message: " + inputMessageGuid);
                var uploadPhotoOutputMessage = uploadPhotoOutputMessageListener.WaitForMessage();
                _logger.LogInformation("Broker. UploadPhotos. Recieve output message: " + inputMessageGuid);
                if (uploadPhotoOutputMessage.Result == OutputMessageResult.Success)
                {
                    uploadedPhotoIds.Add(uploadPhotoOutputMessage.PhotoId);
                }
            }
            if (uploadedPhotoIds.Count == request.Files.Count())
            {
                return new UploadPhotosReponse { Success = true, PhotoIds = uploadedPhotoIds };
            }
            else if (uploadedPhotoIds.Count > 0)
            {
                return new UploadPhotosReponse { Success = true, PhotoIds = uploadedPhotoIds };
            }
            else
            {
                return new UploadPhotosReponse { Success = false };
            }
        }
    }
}
