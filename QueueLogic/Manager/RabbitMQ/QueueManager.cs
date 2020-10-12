using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public ILogger Logger { get; set; }

        public QueueManager()
        {
            _connectionFactory = QueueConnectionFactory.MakeConnectionFactory();
            _connection = _connectionFactory.TryCreateConnection();
            _model = _connection.CreateModel();
            _consumers = new ConcurrentDictionary<string, MessageConsumer>();
        }

        public void SendMessage(string queueName, Message message)
        {
            var props = _model.CreateBasicProperties();
            props.Headers = MessageFactory.GetPropertiesHeaders(message);
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
    }

    class MessageConsumer : AbstractConsumer
    {
        public List<MessageConsumerCallback> Callbacks { get; private set; }

        public MessageConsumer(MessageConsumerCallback callback)
        {
            Callbacks = new List<MessageConsumerCallback> { callback };
        }

        public override void HandleBasicDeliver(
            string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties prop, ReadOnlyMemory<byte> body)
        {
            var message = MessageFactory.MakeMessage(prop, body);
            Callbacks.ForEach(callback => callback(message));
        }
    }

    static class MessageFactory
    {
        public static Dictionary<string, object> GetPropertiesHeaders(Message message)
        {
            var headers = new Dictionary<string, object>();
            headers.Add(MessageFieldConstants.MessageType, message.GetType().AssemblyQualifiedName);
            headers.Add(MessageFieldConstants.MessageChainId, message.ChainId.Value);

            return headers;
        }

        public static Message MakeMessage(IBasicProperties prop, ReadOnlyMemory<byte> body)
        {
            var messageTypeName = prop.GetHeaderValue(MessageFieldConstants.MessageType);
            var message = MessageSerialization.FromBytes(messageTypeName, body);
            return message;
        }
    }
}
