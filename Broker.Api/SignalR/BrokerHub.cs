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
            var clientId = new MessageClientId(clientIdObject.ToString());
            if (_connectionIdDictionary.ContainsKey(clientId))
            {
                _connectionIdDictionary.Remove(clientId);
            }
            _connectionIdDictionary.Add(clientId, Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var key = _connectionIdDictionary.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (key != null)
            {
                _connectionIdDictionary.Remove(key);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task LoginResponse(object responseContainerJson)
        {
            var responseContainer = JsonSerializer.Deserialize<ReponseContainer>(responseContainerJson.ToString());
            var connectionId = GetConnectionIdOrNull(responseContainer.MessageClientId);
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("LoginResponse", responseContainer.Response);
            }
        }

        public async Task GetPhotosResponse(object responseContainerJson)
        {
            var responseContainer = JsonSerializer.Deserialize<ReponseContainer>(responseContainerJson.ToString());
            var connectionId = GetConnectionIdOrNull(responseContainer.MessageClientId);
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("GetPhotosResponse", responseContainer.Response);
            }
        }

        public async Task GetPhotoResponse(object responseContainerJson)
        {
            var responseContainer = JsonSerializer.Deserialize<ReponseContainer>(responseContainerJson.ToString());
            var connectionId = GetConnectionIdOrNull(responseContainer.MessageClientId);
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("GetPhotoResponse", responseContainer.Response);
            }
        }

        public async Task UploadPhotosResponse(object responseContainerJson)
        {
            var responseContainer = JsonSerializer.Deserialize<ReponseContainer>(responseContainerJson.ToString());
            var connectionId = GetConnectionIdOrNull(responseContainer.MessageClientId);
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("UploadPhotosResponse", responseContainer.Response);
            }
        }

        private string GetConnectionIdOrNull(MessageClientId messageClientId)
        {
            return _connectionIdDictionary.ContainsKey(messageClientId) ? _connectionIdDictionary[messageClientId] : null;
        }
    }
}
