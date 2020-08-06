using System;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PhotoBank.QueueLogic.Manager
{
    class BasicConsumerEventArgs : EventArgs
    {
        public Message Message { get; internal set; }
    }

    class BasicConsumer : IBasicConsumer
    {
        public IModel Model { get; set; }

        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;

        public void HandleBasicCancel(string consumerTag)
        {
        }

        public void HandleBasicCancelOk(string consumerTag)
        {
        }

        public void HandleBasicConsumeOk(string consumerTag)
        {
        }

        public event EventHandler<BasicConsumerEventArgs> OnReceiveMessage;

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var messageTypeName = properties.GetHeaderValue(MessageFieldConstants.MessageType);
            var message = (Message)BinarySerialization.FromBytes(messageTypeName, body);
            if (OnReceiveMessage != null) OnReceiveMessage(this, new BasicConsumerEventArgs { Message = message });
        }

        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
        }
    }
}
