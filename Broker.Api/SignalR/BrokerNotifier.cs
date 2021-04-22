using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using PhotoBank.Broker.Api.Contracts;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api.SignalR
{
    public class BrokerNotifier
    {
        public static readonly BrokerNotifier Instance = new BrokerNotifier();

        private HubConnection _hubConnection;
        private readonly OutputMessageConvertersCollection _converters;

        private BrokerNotifier()
        {
            _converters = new OutputMessageConvertersCollection();
        }

        public async void Init(IQueueManager queueManager)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:44364/hub").Build();
            await _hubConnection.StartAsync();
            queueManager.AddMessageConsumer(BrokerSettings.ResultQueue, OnMessageConsume);
        }

        private async void OnMessageConsume(Message message)
        {
            var response = _converters.ToResponse((OutputMessage)message);
            var callbackMethodName = response.GetType().Name;
            var reponseContainer = new ReponseContainer { MessageClientId = message.ClientId, Response = response };
            var reponseContainerSerialized = JsonSerializer.Serialize(reponseContainer);
            await _hubConnection.InvokeAsync(callbackMethodName, reponseContainerSerialized);
        }
    }
}
