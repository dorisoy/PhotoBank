using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly ConcurrentDictionary<string, MessageConsumer> _consumers;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Message>> _messagesDictionary;

        public ILogger Logger { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            _connection = _connectionFactory.TryCreateConnection();
            _model = _connection.CreateModel();
            _consumers = new ConcurrentDictionary<string, MessageConsumer>();
            _messagesDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, Message>>();
        }

        public void SendMessage(string queueName, Message message)
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

        public void AddMessageConsumer(string queueName, MessageConsumerCallback callback)
        {
            if (_consumers.ContainsKey(queueName) == false)
            {
                var consumer = new MessageConsumer(callback);
                _consumers.TryAdd(queueName, consumer);
                _model.BasicConsume(queueName, true, consumer);
            }
            else
            {
                _consumers[queueName].Callbacks.Add(callback);
            }
        }

        public TMessage WaitForMessage<TMessage>(string queueName, string messageGuid) where TMessage : Message
        {
            if (_messagesDictionary.ContainsKey(queueName) == false)
            {
                _messagesDictionary.TryAdd(queueName, new ConcurrentDictionary<string, Message>());
                var consumer = new MessageConsumer(message =>
                {
                    _messagesDictionary[queueName].TryAdd(message.Guid, message);
                });
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

    class MessageConsumer : AbstractConsumer
    {
        public List<MessageConsumerCallback> Callbacks { get; private set; }

        public MessageConsumer(MessageConsumerCallback callback)
        {
            Callbacks = new List<MessageConsumerCallback> { callback };
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var messageTypeName = properties.GetHeaderValue(MessageFieldConstants.MessageType);
            var message = MessageSerialization.FromBytes(messageTypeName, body);
            Callbacks.ForEach(callback => callback(message));
        }
    }
}
