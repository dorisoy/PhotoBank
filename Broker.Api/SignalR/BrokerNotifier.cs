using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.SignalR
{
    public class BrokerNotifier
    {
        public static readonly BrokerNotifier Instance = new BrokerNotifier();

        private BrokerNotifier()
        {
        }

        public void SetQueueManager(IQueueManager queueManager)
        {
            queueManager.AddMessageConsumer(BrokerSettings.ResultQueue, OnMessageConsume);
        }

        private async void OnMessageConsume(Message message)
        {
            var callbackMethodName = GetCallbackMethodName(message);
            var response = MakeResponse(message);
            var hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:44364/hub").Build();
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync(callbackMethodName, JsonSerializer.Serialize(new ReponseContainer { MessageClientId = message.ClientId, Response = response }));
        }

        private string GetCallbackMethodName(Message message)
        {
            if (message is LoginOutputMessage)
            {
                return "LoginResponse";
            }

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

            return null;
        }
    }
}
