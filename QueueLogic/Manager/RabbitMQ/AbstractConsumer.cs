using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    class AbstractConsumer : IBasicConsumer
    {
        public IModel Model { get; set; }

        public event EventHandler<ConsumerEventArgs> ConsumerCancelled;

        public virtual void HandleBasicCancel(string consumerTag)
        {
        }

        public virtual void HandleBasicCancelOk(string consumerTag)
        {
        }

        public virtual void HandleBasicConsumeOk(string consumerTag)
        {
        }

        public virtual void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
        }

        public virtual void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
        }
    }
}
