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
            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    var messageContainer = model.BasicGet(queueName, true);
                    if (messageContainer != null)
                    {
                        var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageType);
                        var message = (Message)BinarySerialization.FromBytes(messageTypeName, messageContainer.Body);
                        return message;
                    }
                    else
                    {
                        Thread.Sleep(WaitTimeout);
                    }
                }
            }
        }

        public TMessage WaitFor<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            var factory = MakeConnectionFactory();
            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    BasicGetResult messageContainer = null;
                    while ((messageContainer = model.BasicGet(queueName, false)) != null)
                    {
                        var messageContainerGuid = messageContainer.BasicProperties.GetHeaderValue(MessageGuid);
                        if (messageGuid == messageContainerGuid)
                        {
                            model.BasicAck(messageContainer.DeliveryTag, false); // отметка, что сообщение получено
                            var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageType);
                            var message = (TMessage)BinarySerialization.FromBytes(messageTypeName, messageContainer.Body);
                            return message;
                        }
                    }
                    Thread.Sleep(WaitTimeout);
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
