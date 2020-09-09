using System.Collections.Generic;
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
        private readonly IConnection _connection;
        private readonly IModel _model;

        public ILogger Logger { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            _connection = _connectionFactory.TryCreateConnection();
            _model = _connection.CreateModel();
        }

        public void Send(string queueName, Message message)
        {
            var props = _model.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add(MessageFieldConstants.MessageType, message.GetType().AssemblyQualifiedName);
            props.Headers.Add(MessageFieldConstants.MessageGuid, message.Guid);
            lock (_lockObject)
            {
                _model.BasicPublish("", queueName, props, MessageSerialization.ToBytes(message));
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
