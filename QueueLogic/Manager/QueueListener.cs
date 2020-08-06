using System;
using System.Collections.Concurrent;
using PhotoBank.QueueLogic.Contracts;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager
{
    class QueueListener : IQueueListener
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly BasicConsumer _basicConsumer;
        private readonly ConcurrentQueue<Message> _messages;

        public QueueListener(string queueName, bool autoAck, ConnectionFactory factory)
        {
            _messages = new ConcurrentQueue<Message>();
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnReceiveMessage += OnReceiveMessage;
            _model.BasicConsume(queueName, autoAck, _basicConsumer);
        }

        private void OnReceiveMessage(object sender, BasicConsumerEventArgs e)
        {
            _messages.Enqueue(e.Message);
        }

        public Message WaitForMessage()
        {
            Message message;
            while (_messages.TryDequeue(out message) == false) ;
            return message;
        }

        public void Dispose()
        {
            if (_model != null) _model.Dispose();
            if (_connection != null) _connection.Dispose();
        }
    }
}
