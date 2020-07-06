using System;
using System.Collections.Generic;
using System.Threading;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager
{
    public class QueueManager : IQueueManager
    {
        public TimeSpan WaitTimeout { get; set; }

        private static readonly string MessageType = "MessageType";
        private static readonly string MessageGuid = "MessageGuid";

        public QueueManager()
        {
            WaitTimeout = TimeSpan.FromSeconds(1);
        }

        public void Send(string queueName, Message messsage)
        {
            var factory = MakeConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var model = connection.CreateModel())
            {
                var props = model.CreateBasicProperties();
                props.Headers = new Dictionary<string, object>();
                props.Headers.Add(MessageType, messsage.GetType().AssemblyQualifiedName);
                props.Headers.Add(MessageGuid, messsage.Guid);
                model.BasicPublish("", queueName, props, BinarySerialization.ToBytes(messsage));
            }
        }

        public Message Wait(string queueName)
        {
            var factory = MakeConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var model = connection.CreateModel())
            {
                while (true)
                {
                    var messageContainer = model.BasicGet(queueName, true);
                    if (messageContainer == null)
                    {
                        Thread.Sleep(WaitTimeout);
                        continue;
                    }
                    var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageType);
                    var message = (Message)BinarySerialization.FromBytes(messageTypeName, messageContainer.Body);
                    return message;
                }
            }
        }

        public TMessage WaitFor<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            var factory = MakeConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var model = connection.CreateModel())
            {
                while (true)
                {
                    var messageContainer = model.BasicGet(queueName, false);
                    if (messageContainer == null)
                    {
                        Thread.Sleep(WaitTimeout);
                        continue;
                    }
                    var messageContainerGuid = messageContainer.BasicProperties.GetHeaderValue(MessageGuid);
                    if (messageGuid == messageContainerGuid)
                    {
                        model.BasicAck(messageContainer.DeliveryTag, false);
                        var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageType);
                        var message = (TMessage)BinarySerialization.FromBytes(messageTypeName, messageContainer.Body);
                        return message;
                    }
                }
            }
        }

        private ConnectionFactory MakeConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "vinge",
                Password = "vinge",
            };
        }
    }
}
