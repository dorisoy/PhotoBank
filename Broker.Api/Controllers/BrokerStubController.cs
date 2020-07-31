using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.Broker.Api.Contracts;

namespace PhotoBank.Broker.Api.Controllers
{
    [ApiController]
    [Route("api/stub")]
    [EnableCors("TCAPolicy")]
    public class BrokerStubController : ControllerBase
    {
        [HttpPost]
        [Route("createUser")]
        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            if (request.Login == "vinge" && request.Password == "12345")
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
            if (request.Login == "vinge" && request.Password == "12345")
            {
                return new LoginResponse { Success = true, Token = "qwertyuiop" };
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
            if (request.Login == "vinge" && request.Token == "qwertyuiop")
            {
                return new GetPhotosResponse { Success = true, PhotoIds = new[] { 1, 2 } };
            }
            else
            {
                return new GetPhotosResponse { Success = false };
            }
        }

        [HttpGet]
        [Route("getPhoto")]
        public IActionResult GetPhotoGet(string login, string token, int photoId)
        {
            if (login == "vinge" && token == "qwertyuiop" && photoId == 1)
            {
                return File(System.IO.File.ReadAllBytes(@"D:\Projects\PhotoBank\Photo.Service\Database\DSC_8671.jpg"), "image/jpeg");
            }
            else if (login == "vinge" && token == "qwertyuiop" && photoId == 2)
            {
                return File(System.IO.File.ReadAllBytes(@"D:\Projects\PhotoBank\Photo.Service\Database\DSC_9918.jpg"), "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("getPhoto")]
        public IActionResult GetPhotoPost(GetPhotoRequest request)
        {
            if (request.Login == "vinge" && request.Token == "qwertyuiop" && request.PhotoId == 1)
            {
                return File(System.IO.File.ReadAllBytes(@"D:\Projects\PhotoBank\Photo.Service\Database\DSC_8671.jpg"), "image/jpeg");
            }
            else if (request.Login == "vinge" && request.Token == "qwertyuiop" && request.PhotoId == 2)
            {
                return File(System.IO.File.ReadAllBytes(@"D:\Projects\PhotoBank\Photo.Service\Database\DSC_9918.jpg"), "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("uploadPhotos")]
        public UploadPhotoReponse UploadPhotos(UploadPhotoRequest request)
        {
            var bytes1 = System.IO.File.ReadAllBytes(@"C:\Users\Kolya\Desktop\1.png");
            var bytes2 = Convert.FromBase64String(request.Files.First());
            if (request.Login == "vinge" && request.Token == "qwertyuiop")
            {
                return new UploadPhotoReponse { Success = false };
            }
            else
            {
                return new UploadPhotoReponse { Success = false };
            }
        }
    }
}
