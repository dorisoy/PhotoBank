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
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
     //   private IModel _model;

        public TimeSpan WaitTimeout { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            _connection = _connectionFactory.CreateConnection();
         //   _model = _connection.CreateModel();
            WaitTimeout = TimeSpan.FromSeconds(1);
        }

        public void Send(string queueName, Message messsage)
        {
            //using (var connection = _connectionFactory.CreateConnection())
            using (var model = _connection.CreateModel())
            {
                var props = model.CreateBasicProperties();
                props.Headers = new Dictionary<string, object>();
                props.Headers.Add(MessageFieldConstants.MessageType, messsage.GetType().AssemblyQualifiedName);
                props.Headers.Add(MessageFieldConstants.MessageGuid, messsage.Guid);
                model.BasicPublish("", queueName, props, BinarySerialization.ToBytes(messsage));
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

        private object _lockObject = new object();
        public IQueueListener CreateQueueListener(string queueName)
        {
            //return new QueueListener(queueName, _connectionFactory);
            lock (_lockObject)
            {
                return new QueueListener(queueName, _connection);
            }
        }

        public IQueueMessageListener<TMessage> CreateQueueMessageListener<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            //return new QueueMessageListener<TMessage>(queueName, messageGuid, _connectionFactory);
            lock (_lockObject)
            {
                return new QueueMessageListener<TMessage>(queueName, messageGuid, _connection);
            }
        }
    }
}
