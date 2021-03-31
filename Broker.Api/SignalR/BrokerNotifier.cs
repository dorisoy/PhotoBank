using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Photo.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.SignalR
{
    public class BrokerNotifier
    {
        public static readonly BrokerNotifier Instance = new BrokerNotifier();

        private HubConnection _hubConnection;

        private BrokerNotifier()
        {
        }

        public async void Init(IQueueManager queueManager)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:44364/hub").Build();
            await _hubConnection.StartAsync();
            queueManager.AddMessageConsumer(BrokerSettings.ResultQueue, OnMessageConsume);
        }

        private async void OnMessageConsume(Message message)
        {
            var callbackMethodName = GetCallbackMethodName(message);
            var response = MakeResponse(message);
            var reponseContainer = new ReponseContainer { MessageClientId = message.ClientId, Response = response };
            var reponseContainerSerialized = JsonSerializer.Serialize(reponseContainer);
            await _hubConnection.InvokeAsync(callbackMethodName, reponseContainerSerialized);
        }

        private string GetCallbackMethodName(Message message)
        {
            if (message is LoginOutputMessage) return "LoginResponse";
            if (message is GetUserInfoOutputMessage) return "GetUserInfoResponse";
            if (message is SetUserInfoOutputMessage) return "SetUserInfoResponse";
            if (message is LoadUserPictureOutputMessage) return "LoadUserPictureResponse";
            if (message is SetUserPictureOutputMessage) return "SetUserPictureResponse";
            if (message is GetPhotosOutputMessage) return "GetPhotosResponse";
            if (message is GetPhotoOutputMessage) return "GetPhotoResponse";
            if (message is UploadPhotoOutputMessage) return "UploadPhotosResponse";
            if (message is DeletePhotoOutputMessage) return "DeletePhotoResponse";
            if (message is GetPhotoAdditionalInfoOutputMessage) return "GetPhotoAdditionalInfoResponse";
            if (message is SetPhotoAdditionalInfoOutputMessage) return "SetPhotoAdditionalInfoResponse";

            return null;
        }

        private object MakeResponse(Message message)
        {
            if (message is LoginOutputMessage)
            {
                var outputMessage = (LoginOutputMessage)message;
                return new LoginResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    Token = outputMessage.Token
                };
            }

            if (message is GetUserInfoOutputMessage)
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

            if (message is SetUserInfoOutputMessage)
            {
                var outputMessage = (SetUserInfoOutputMessage)message;
                return new GetUserInfoResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success
                };
            }

            if (message is LoadUserPictureOutputMessage)
            {
                var outputMessage = (LoadUserPictureOutputMessage)message;
                return new LoadUserPictureResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PictureBase64Content = outputMessage.PictureBase64Content,
                    NewPictureId = outputMessage.NewPictureId
                };
            }

            if (message is SetUserPictureOutputMessage)
            {
                var outputMessage = (SetUserPictureOutputMessage)message;
                return new SetUserPictureResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success
                };
            }

            if (message is GetPhotosOutputMessage)
            {
                var outputMessage = (GetPhotosOutputMessage)message;
                return new GetPhotosResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PhotoIds = outputMessage.PhotoIds
                };
            }

            if (message is GetPhotoOutputMessage)
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

            if (message is UploadPhotoOutputMessage)
            {
                var outputMessage = (UploadPhotoOutputMessage)message;
                return new UploadPhotosResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PhotoId = outputMessage.PhotoId
                };
            }

            if (message is DeletePhotoOutputMessage)
            {
                var outputMessage = (DeletePhotoOutputMessage)message;
                return new DeletePhotoResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PhotoId = outputMessage.PhotoId
                };
            }

            if (message is GetPhotoAdditionalInfoOutputMessage)
            {
                var outputMessage = (GetPhotoAdditionalInfoOutputMessage)message;
                return new GetPhotoAdditionalInfoResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PhotoId = outputMessage.PhotoId,
                    AdditionalInfo = outputMessage.AdditionalInfo
                };
            }

            if (message is SetPhotoAdditionalInfoOutputMessage)
            {
                var outputMessage = (SetPhotoAdditionalInfoOutputMessage)message;
                return new SetPhotoAdditionalInfoResponse
                {
                    Success = outputMessage.Result == OutputMessageResult.Success,
                    PhotoId = outputMessage.PhotoId
                };
            }

            return null;
        }
    }
}
