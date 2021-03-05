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
                var loginOutputMessage = (LoginOutputMessage)message;
                return new LoginResponse
                {
                    Success = loginOutputMessage.Result == OutputMessageResult.Success,
                    Token = loginOutputMessage.Token
                };
            }

            if (message is GetPhotosOutputMessage)
            {
                var getPhotosOutputMessage = (GetPhotosOutputMessage)message;
                return new GetPhotosResponse
                {
                    Success = getPhotosOutputMessage.Result == OutputMessageResult.Success,
                    PhotoIds = getPhotosOutputMessage.PhotoIds
                };
            }

            if (message is GetPhotoOutputMessage)
            {
                var getPhotosOutputMessage = (GetPhotoOutputMessage)message;
                return new GetPhotoResponse
                {
                    Success = getPhotosOutputMessage.Result == OutputMessageResult.Success,
                    PhotoId = getPhotosOutputMessage.PhotoId,
                    FileBase64Content = getPhotosOutputMessage.FileBase64Content
                };
            }

            if (message is UploadPhotoOutputMessage)
            {
                var uploadPhotoOutputMessage = (UploadPhotoOutputMessage)message;
                return new UploadPhotosResponse
                {
                    Success = uploadPhotoOutputMessage.Result == OutputMessageResult.Success,
                    PhotoId = uploadPhotoOutputMessage.PhotoId
                };
            }

            if (message is DeletePhotoOutputMessage)
            {
                var deletePhotoOutputMessage = (DeletePhotoOutputMessage)message;
                return new DeletePhotoResponse
                {
                    Success = deletePhotoOutputMessage.Result == OutputMessageResult.Success,
                    PhotoId = deletePhotoOutputMessage.PhotoId
                };
            }

            if (message is GetPhotoAdditionalInfoOutputMessage)
            {
                var getPhotoAdditionalInfoOutputMessage = (GetPhotoAdditionalInfoOutputMessage)message;
                return new GetPhotoAdditionalInfoResponse
                {
                    Success = getPhotoAdditionalInfoOutputMessage.Result == OutputMessageResult.Success,
                    PhotoId = getPhotoAdditionalInfoOutputMessage.PhotoId,
                    AdditionalInfo = getPhotoAdditionalInfoOutputMessage.AdditionalInfo
                };
            }

            if (message is SetPhotoAdditionalInfoOutputMessage)
            {
                var setPhotoAdditionalInfoOutputMessage = (SetPhotoAdditionalInfoOutputMessage)message;
                return new SetPhotoAdditionalInfoResponse
                {
                    Success = setPhotoAdditionalInfoOutputMessage.Result == OutputMessageResult.Success,
                    PhotoId = setPhotoAdditionalInfoOutputMessage.PhotoId
                };
            }

            return null;
        }
    }
}
