﻿using System;
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
        [AuthenticationFilter]
        public GetPhotosResponse GetPhotos(GetPhotosRequest request)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var getPhotosInputMessage = new GetPhotosInputMessage(inputMessageGuid)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
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

        [HttpGet]
        [Route("getPhoto")]
        [AuthenticationFilter]
        public IActionResult GetPhoto(string login, string token, int photoId)
        {
            var inputMessageGuid = Guid.NewGuid().ToString();
            var getPhotoInputMessage = new GetPhotoInputMessage(inputMessageGuid)
            {
                PhotoId = photoId
            };
            _queueManager.Send(PhotoSettings.PhotoInputQueue, getPhotoInputMessage);
            var getPhotoOutputMessage = _queueManager.WaitFor<GetPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
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
        [AuthenticationFilter]
        public UploadPhotosReponse UploadPhotos(UploadPhotosRequest request)
        {
            if ((request.Files ?? Enumerable.Empty<string>()).Any() == false)
            {
                return new UploadPhotosReponse { Result = UploadPhotoResult.NoOne };
            }
            var inputMessageGuid = Guid.NewGuid().ToString();
            var uploadedPhotoIds = new List<int>();
            foreach (var fileBase64Content in request.Files)
            {
                var uploadPhotoInputMessage = new UploadPhotoInputMessage(inputMessageGuid)
                {
                    UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                    FileBase64Content = fileBase64Content
                };
                _queueManager.Send(PhotoSettings.PhotoInputQueue, uploadPhotoInputMessage);
                var uploadPhotoOutputMessage = _queueManager.WaitFor<UploadPhotoOutputMessage>(BrokerSettings.ResultQueue, inputMessageGuid);
                if (uploadPhotoOutputMessage.Result == OutputMessageResult.Success)
                {
                    uploadedPhotoIds.Add(uploadPhotoOutputMessage.PhotoId);
                }
            }
            if (uploadedPhotoIds.Count == request.Files.Count())
            {
                return new UploadPhotosReponse { Result = UploadPhotoResult.AllPhotos, PhotoIds = uploadedPhotoIds };
            }
            else if (uploadedPhotoIds.Count > 0)
            {
                return new UploadPhotosReponse { Result = UploadPhotoResult.Partially, PhotoIds = uploadedPhotoIds };
            }
            else
            {
                return new UploadPhotosReponse { Result = UploadPhotoResult.NoOne };
            }
        }
    }
}
