using System;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Broker.Api.SignalR
{
    public interface IOutputMessageConverter
    {
        Type MessageType { get; }
        object ToResponse(OutputMessage message);
    }

    public class LoginOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(LoginOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (LoginOutputMessage)message;
            return new LoginResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                Token = outputMessage.Token
            };
        }
    }

    public class GetUserInfoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetUserInfoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetUserInfoOutputMessage)message;
            return new GetUserInfoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                Name = outputMessage.Name,
                EMail = outputMessage.EMail,
                About = outputMessage.About,
                PictureBase64Content = outputMessage.PictureBase64Content
            };
        }
    }

    public class SetUserInfoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(SetUserInfoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (SetUserInfoOutputMessage)message;
            return new SetUserInfoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success
            };
        }
    }

    public class LoadUserPictureOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(LoadUserPictureOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (LoadUserPictureOutputMessage)message;
            return new LoadUserPictureResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PictureBase64Content = outputMessage.PictureBase64Content,
                NewPictureId = outputMessage.NewPictureId
            };
        }
    }

    public class SetUserPictureOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(SetUserPictureOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (SetUserPictureOutputMessage)message;
            return new SetUserPictureResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success
            };
        }
    }

    public class GetPhotosOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetPhotosOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetPhotosOutputMessage)message;
            return new GetPhotosResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoIds = outputMessage.PhotoIds
            };
        }
    }

    public class GetPhotoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetPhotoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetPhotoOutputMessage)message;
            return new GetPhotoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoId = outputMessage.PhotoId,
                FileBase64Content = outputMessage.FileBase64Content,
                CreateDate = outputMessage.CreateDate
            };
        }
    }

    public class UploadPhotoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(UploadPhotoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (UploadPhotoOutputMessage)message;
            return new UploadPhotosResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoId = outputMessage.PhotoId
            };
        }
    }

    public class DeletePhotoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(DeletePhotoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (DeletePhotoOutputMessage)message;
            return new DeletePhotoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoId = outputMessage.PhotoId
            };
        }
    }

    public class GetPhotoAdditionalInfoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetPhotoAdditionalInfoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetPhotoAdditionalInfoOutputMessage)message;
            return new GetPhotoAdditionalInfoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoId = outputMessage.PhotoId,
                AdditionalInfo = outputMessage.AdditionalInfo
            };
        }
    }

    public class SetPhotoAdditionalInfoOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(SetPhotoAdditionalInfoOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (SetPhotoAdditionalInfoOutputMessage)message;
            return new SetPhotoAdditionalInfoResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                PhotoId = outputMessage.PhotoId
            };
        }
    }

    public class GetUserAlbumsOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetUserAlbumsOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetUserAlbumsOutputMessage)message;
            return new GetUserAlbumsResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                Albums = outputMessage.Albums
            };
        }
    }

    public class CreateUserAlbumsOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(CreateUserAlbumsOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (CreateUserAlbumsOutputMessage)message;
            return new CreateUserAlbumsResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                Albums = outputMessage.Albums
            };
        }
    }

    public class DeleteUserAlbumsOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(DeleteUserAlbumsOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (DeleteUserAlbumsOutputMessage)message;
            return new DeleteUserAlbumsResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success
            };
        }
    }

    public class GetPhotoAlbumsOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(GetPhotoAlbumsOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (GetPhotoAlbumsOutputMessage)message;
            return new GetPhotoAlbumsResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success,
                AlbumsId = outputMessage.AlbumsId
            };
        }
    }

    public class SetPhotoAlbumsOutputMessageConverter : IOutputMessageConverter
    {
        public Type MessageType { get { return typeof(SetPhotoAlbumsOutputMessage); } }

        public object ToResponse(OutputMessage message)
        {
            var outputMessage = (SetPhotoAlbumsOutputMessage)message;
            return new SetPhotoAlbumsResponse
            {
                Success = outputMessage.Result == OutputMessageResult.Success
            };
        }
    }
}
