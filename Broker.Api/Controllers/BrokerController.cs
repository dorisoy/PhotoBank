using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Authentication;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Logger.Common;
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
        private readonly IMessageLogger _logger;

        public BrokerController(IAuthenticationManager authenticationManager, IQueueManager queueManager, IMessageLogger logger)
        {
            _authenticationManager = authenticationManager;
            _queueManager = queueManager;
            _logger = logger;
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
        [Route("getUserInfo")]
        [CheckAuthentication]
        public IActionResult GetUserInfo(GetUserInfoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new GetUserInfoInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
            };
            _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("setUserInfo")]
        [CheckAuthentication]
        public IActionResult SetUserInfo(SetUserInfoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new SetUserInfoInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                Name = request.Name,
                EMail = request.EMail,
                About = request.About
            };
            _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("loadUserPicture")]
        [CheckAuthentication]
        public IActionResult LoadUserPicture(LoadUserPictureRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new LoadUserPictureInputMessage(messageClientId, messageChainId)
            {
                PictureBase64Content = request.PictureFile
            };
            _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("setUserPicture")]
        [CheckAuthentication]
        public IActionResult SetUserPicture(SetUserPictureRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.NewPictureId) == false)
            {
                var messageClientId = new MessageClientId(request.ClientId);
                var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
                var inputMessage = new SetUserPictureInputMessage(messageClientId, messageChainId)
                {
                    UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                    NewPictureId = request.NewPictureId
                };
                _queueManager.SendMessage(AuthSettings.AuthInputQueue, inputMessage);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            _logger.Info(messageClientId, messageChainId, String.Format("Запрос Login. {0}", JsonSerializer.Serialize(request)));
            var inputMessage = new LoginInputMessage(messageClientId, messageChainId)
            {
                Login = request.Login,
                Password = request.Password
            };
            _logger.InputMessageCreated(inputMessage);
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
            var inputMessage = new GetPhotosInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                AlbumsId = request.AlbumsId
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getPhoto")]
        [CheckAuthentication]
        public IActionResult GetPhoto(GetPhotoRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new GetPhotoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

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
                var inputMessage = new UploadPhotoInputMessage(messageClientId, messageChainId)
                {
                    UserId = userId,
                    FileBase64Content = fileBase64Content
                };
                _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);
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
            var inputMessage = new DeletePhotoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

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
            var inputMessage = new SetPhotoAdditionalInfoInputMessage(messageClientId, messageChainId)
            {
                PhotoId = request.PhotoId,
                AdditionalInfo = request.AdditionalInfo
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getUserAlbums")]
        [CheckAuthentication]
        public IActionResult GetUserAlbums(GetUserAlbumsRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new GetUserAlbumsInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token)
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("createUserAlbums")]
        [CheckAuthentication]
        public IActionResult CreateUserAlbums(CreateUserAlbumsRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new CreateUserAlbumsInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                NewAlbums = request.NewAlbums
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("deleteUserAlbums")]
        [CheckAuthentication]
        public IActionResult DeleteUserAlbums(DeleteUserAlbumsRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new DeleteUserAlbumsInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                AlbumsId = request.AlbumsId
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("getPhotoAlbums")]
        [CheckAuthentication]
        public IActionResult GetPhotoAlbums(GetPhotoAlbumsRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new GetPhotoAlbumsInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                PhotoId = request.PhotoId
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }

        [HttpPost]
        [Route("setPhotoAlbums")]
        [CheckAuthentication]
        public IActionResult SetPhotoAlbums(SetPhotoAlbumsRequest request)
        {
            var messageClientId = new MessageClientId(request.ClientId);
            var messageChainId = new MessageChainId(Guid.NewGuid().ToString());
            var inputMessage = new SetPhotoAlbumsInputMessage(messageClientId, messageChainId)
            {
                UserId = _authenticationManager.GetUserId(request.Login, request.Token),
                PhotoId = request.PhotoId,
                AlbumsId = request.AlbumsId,
                AlbumsName = request.AlbumsName
            };
            _queueManager.SendMessage(PhotoSettings.PhotoInputQueue, inputMessage);

            return Ok();
        }
    }
}
