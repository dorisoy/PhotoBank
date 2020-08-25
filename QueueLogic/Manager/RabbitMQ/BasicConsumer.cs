using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
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

        public event EventHandler<HandleBasicDeliverEventArgs> OnHandleBasicDeliver;

        public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            if (OnHandleBasicDeliver != null)
            {
                OnHandleBasicDeliver(this, new HandleBasicDeliverEventArgs
                {
                    ConsumerTag = consumerTag,
                    DeliveryTag = deliveryTag,
                    Redelivered = redelivered,
                    Exchange = exchange,
                    RoutingKey = routingKey,
                    Properties = properties,
                    Body = body
                });
            }
        }

        public void HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
        }

        public class HandleBasicDeliverEventArgs : EventArgs
        {
            public string ConsumerTag { get; set; }

            public ulong DeliveryTag { get; set; }

            public bool Redelivered { get; set; }

            public string Exchange { get; set; }

            public string RoutingKey { get; set; }

            public IBasicProperties Properties { get; set; }

            public ReadOnlyMemory<byte> Body { get; set; }
        }
    }
}
