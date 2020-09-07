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
            using (var createUserOutputMessageListener = _queueManager.CreateQueueMessageListener<CreateUserOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid))
            {
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
            using (var loginOutputMessageListener = _queueManager.CreateQueueMessageListener<LoginOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid))
            {
                var outputMessage = loginOutputMessageListener.WaitForMessage();
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
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotosInputMessage);
            using (var getPhotosOutputMessageListener = _queueManager.CreateQueueMessageListener<GetPhotosOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid))
            {
                var getPhotosOutputMessage = getPhotosOutputMessageListener.WaitForMessage();
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
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotoInputMessage);
            using (var getPhotoOutputMessageListener = _queueManager.CreateQueueMessageListener<GetPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid))
            {
                var getPhotoOutputMessage = getPhotoOutputMessageListener.WaitForMessage();
                if (getPhotoOutputMessage.Result == OutputMessageResult.Success)
                {
                    return File(getPhotoOutputMessage.PhotoBytes, "image/jpeg");
                }
                else
                {
                    return NotFound();
                }
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
                _queueManager.Send(PhotoSettings.PhotoInputQueue, uploadPhotoInputMessage);
            }
            foreach (var inputMessageGuid in inputMessageGuidList)
            {
                using (var uploadPhotoOutputMessageListener = _queueManager.CreateQueueMessageListener<UploadPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid))
                {
                    var uploadPhotoOutputMessage = uploadPhotoOutputMessageListener.WaitForMessage();
                    if (uploadPhotoOutputMessage.Result == OutputMessageResult.Success)
                    {
                        uploadedPhotoIds.Add(uploadPhotoOutputMessage.PhotoId);
                    }
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
