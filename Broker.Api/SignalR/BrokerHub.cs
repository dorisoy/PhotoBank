using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Broker.Api.SignalR
{
    public class BrokerHub : Hub
    {
        private static Dictionary<MessageClientId, string> _connectionIdDictionary = new Dictionary<MessageClientId, string>();

        public void Register(object clientIdObject)
        {
            lock (_connectionIdDictionary)
            {
                var clientId = new MessageClientId(clientIdObject.ToString());
                if (_connectionIdDictionary.ContainsKey(clientId))
                {
                    _connectionIdDictionary.Remove(clientId);
                }
                _connectionIdDictionary.Add(clientId, Context.ConnectionId);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            lock (_connectionIdDictionary)
            {
                var key = _connectionIdDictionary.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
                if (key != null)
                {
                    _connectionIdDictionary.Remove(key);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task LoginResponse(object responseContainerJson)
        {
            await SendAsync("LoginResponse", responseContainerJson);
        }

        public async Task GetUserInfoResponse(object responseContainerJson)
        {
            await SendAsync("GetUserInfoResponse", responseContainerJson);
        }

        public async Task SetUserInfoResponse(object responseContainerJson)
        {
            await SendAsync("SetUserInfoResponse", responseContainerJson);
        }

        public async Task LoadUserPictureResponse(object responseContainerJson)
        {
            await SendAsync("LoadUserPictureResponse", responseContainerJson);
        }

        public async Task SetUserPictureResponse(object responseContainerJson)
        {
            await SendAsync("SetUserPictureResponse", responseContainerJson);
        }

        public async Task GetPhotosResponse(object responseContainerJson)
        {
            await SendAsync("GetPhotosResponse", responseContainerJson);
        }

        public async Task GetPhotoResponse(object responseContainerJson)
        {
            await SendAsync("GetPhotoResponse", responseContainerJson);
        }

        public async Task UploadPhotosResponse(object responseContainerJson)
        {
            await SendAsync("UploadPhotosResponse", responseContainerJson);
        }

        public async Task DeletePhotoResponse(object responseContainerJson)
        {
            await SendAsync("DeletePhotoResponse", responseContainerJson);
        }

        public async Task GetPhotoAdditionalInfoResponse(object responseContainerJson)
        {
            await SendAsync("GetPhotoAdditionalInfoResponse", responseContainerJson);
        }

        public async Task SetPhotoAdditionalInfoResponse(object responseContainerJson)
        {
            await SendAsync("SetPhotoAdditionalInfoResponse", responseContainerJson);
        }

        public async Task GetUserAlbumsResponse(object responseContainerJson)
        {
            await SendAsync("GetUserAlbumsResponse", responseContainerJson);
        }

        public async Task CreateUserAlbumsResponse(object responseContainerJson)
        {
            await SendAsync("CreateUserAlbumsResponse", responseContainerJson);
        }

        public async Task DeleteUserAlbumsResponse(object responseContainerJson)
        {
            await SendAsync("DeleteUserAlbumsResponse", responseContainerJson);
        }

        public async Task GetPhotoAlbumsResponse(object responseContainerJson)
        {
            await SendAsync("GetPhotoAlbumsResponse", responseContainerJson);
        }

        public async Task SetPhotoAlbumsResponse(object responseContainerJson)
        {
            await SendAsync("SetPhotoAlbumsResponse", responseContainerJson);
        }

        private async Task SendAsync(string methodName, object responseContainerJson)
        {
            string connectionId;
            ReponseContainer responseContainer;
            lock (_connectionIdDictionary)
            {
                responseContainer = JsonSerializer.Deserialize<ReponseContainer>(responseContainerJson.ToString());
                connectionId = GetConnectionIdOrNull(responseContainer.MessageClientId);
            }
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync(methodName, responseContainer.Response);
            }
        }

        private string GetConnectionIdOrNull(MessageClientId messageClientId)
        {
            return _connectionIdDictionary.ContainsKey(messageClientId) ? _connectionIdDictionary[messageClientId] : null;
        }
    }
}
