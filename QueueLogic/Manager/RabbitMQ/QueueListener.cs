using System;
using System.Collections.Concurrent;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    class QueueListener : IQueueListener
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly BasicConsumer _basicConsumer;
        private readonly ConcurrentQueue<Message> _messages;

        public event EventHandler<ReceiveMessageEventArgs> ReceiveMessage;

        public QueueListener(string queueName, ConnectionFactory factory)
        {
            _messages = new ConcurrentQueue<Message>();
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += HandleBasicDeliver;
            _model.BasicConsume(queueName, true, _basicConsumer);
        }

        private void HandleBasicDeliver(object sender, BasicConsumer.HandleBasicDeliverEventArgs e)
        {
            var messageTypeName = e.Properties.GetHeaderValue(MessageFieldConstants.MessageType);
            var message = (Message)BinarySerialization.FromBytes(messageTypeName, e.Body);
            _messages.Enqueue(message);
            if (ReceiveMessage != null)
            {
                ReceiveMessage(this, new ReceiveMessageEventArgs { Message = message });
            }
        }

        public void Dispose()
        {
            if (_model != null) _model.Dispose();
            if (_connection != null) _connection.Dispose();
        }
    }
}
