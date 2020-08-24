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
        private object _lockObject = new object();
        private ConnectionFactory _connectionFactory;

        public TimeSpan WaitTimeout { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            WaitTimeout = TimeSpan.FromSeconds(1);
        }

        public void Send(string queueName, Message messsage)
        {
            lock (_lockObject)
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    var props = model.CreateBasicProperties();
                    props.Headers = new Dictionary<string, object>();
                    props.Headers.Add(MessageFieldConstants.MessageType, messsage.GetType().AssemblyQualifiedName);
                    props.Headers.Add(MessageFieldConstants.MessageGuid, messsage.Guid);
                    model.BasicPublish("", queueName, props, BinarySerialization.ToBytes(messsage));
                }
            }
        }

        public Message Wait(string queueName)
        {
            var factory = QueueConnectionFactory.MakeConnectionFactory();
            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    var messageContainer = model.BasicGet(queueName, true);
                    if (messageContainer != null)
                    {
                        var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageFieldConstants.MessageType);
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
            var factory = QueueConnectionFactory.MakeConnectionFactory();
            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    BasicGetResult messageContainer = null;
                    while ((messageContainer = model.BasicGet(queueName, false)) != null)
                    {
                        var messageContainerGuid = messageContainer.BasicProperties.GetHeaderValue(MessageFieldConstants.MessageGuid);
                        if (messageGuid == messageContainerGuid)
                        {
                            model.BasicAck(messageContainer.DeliveryTag, false); // отметка, что сообщение получено
                            var messageTypeName = messageContainer.BasicProperties.GetHeaderValue(MessageFieldConstants.MessageType);
                            var message = (TMessage)BinarySerialization.FromBytes(messageTypeName, messageContainer.Body);
                            return message;
                        }
                    }
                    Thread.Sleep(WaitTimeout);
                }
            }
        }

        public IQueueListener CreateQueueListener(string queueName)
        {
            lock (_lockObject)
            {
                return new QueueListener(queueName, _connectionFactory);
            }
        }

        public IQueueMessageListener<TMessage> CreateQueueMessageListener<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            lock (_lockObject)
            {
                return new QueueMessageListener<TMessage>(queueName, messageGuid, _connectionFactory);
            }
        }
    }
}
