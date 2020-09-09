﻿using System.Collections.Generic;
using PhotoBank.QueueLogic.Utils;
using PhotoBank.QueueLogic.Contracts;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    public class QueueManager : IQueueManager
    {
        private object _lockObject = new object();
        private ConnectionFactory _connectionFactory;

        public ILogger Logger { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
        }

        public void Send(string queueName, Message message)
        {
            lock (_lockObject)
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    var props = model.CreateBasicProperties();
                    props.Headers = new Dictionary<string, object>();
                    props.Headers.Add(MessageFieldConstants.MessageType, message.GetType().AssemblyQualifiedName);
                    props.Headers.Add(MessageFieldConstants.MessageGuid, message.Guid);
                    model.BasicPublish("", queueName, props, MessageSerialization.ToBytes(message));
                }
            }
        }

        public IQueueListener CreateQueueListener(string queueName)
        {
            lock (_lockObject)
            {
                return new QueueListener(queueName, _connectionFactory) { Logger = Logger };
            }
        }

        public IQueueMessageListener<TMessage> CreateQueueMessageListener<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            lock (_lockObject)
            {
                return new QueueMessageListener<TMessage>(queueName, messageGuid, _connectionFactory) { Logger = Logger };
            }
        }
    }
}
