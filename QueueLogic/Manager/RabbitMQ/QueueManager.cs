using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    public class QueueManager : IQueueManager
    {
        private readonly object _lockObject = new object();
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Message>> _messagesDictionary;

        public ILogger Logger { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            _connection = _connectionFactory.TryCreateConnection();
            _model = _connection.CreateModel();
            _messagesDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, Message>>();
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

        public TMessage WaitForMessage<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            if (_messagesDictionary.ContainsKey(queueName) == false)
            {
                _messagesDictionary.TryAdd(queueName, new ConcurrentDictionary<string, Message>());
                var consumer = new BasicConsumer();
                consumer.OnHandleBasicDeliver += (s, e) =>
                {
                    var messageTypeName = e.Properties.GetHeaderValue(MessageFieldConstants.MessageType);
                    var message = MessageSerialization.FromBytes(messageTypeName, e.Body);
                    if (message != null)
                    {
                        _model.BasicAck(e.DeliveryTag, false); // отметка, что сообщение получено
                        _messagesDictionary[queueName].TryAdd(message.Guid, message);
                    }
                };
                _model.BasicConsume(queueName, false, consumer);
            }
            ConcurrentDictionary<string, Message> messages;
            _messagesDictionary.TryGetValue(queueName, out messages);
            while (messages.ContainsKey(messageGuid) == false)
            {
                Thread.Sleep(200);
            }

            Message message;
            messages.TryRemove(messageGuid, out message);

            return (TMessage)message;
        }
    }
}
