using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using PhotoBank.Auth.Contracts;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.Broker.Api.Utils;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.Hubs
{
    public class MainHub : Hub
    {
        public MainHub(IQueueManager queueManager)
        {
            queueManager.AddMessageConsumer(BrokerSettings.ResultQueue, OnMessageConsume);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        private void OnMessageConsume(Message message)
        {
            var requestClientId = ClientIdBuidler.Build(Context.GetHttpContext());
            var messageClientId = message.ClientId;
            if (requestClientId.Equals(messageClientId))
            {
                var callbackMethodName = GetCallbackMethodName(message);
                var response = MakeResponse(message);
                Clients.Caller.SendAsync(callbackMethodName, response);
            }
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
