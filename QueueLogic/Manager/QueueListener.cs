using System.Collections.Concurrent;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager
{
    class QueueListener : IQueueListener
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly BasicConsumer _basicConsumer;
        private readonly ConcurrentQueue<Message> _messages;

        public QueueListener(string queueName, ConnectionFactory factory)
        {
            _messages = new ConcurrentQueue<Message>();
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += HandleBasicDeliver;
            _model.BasicConsume(queueName, true, _basicConsumer);
        }

        public QueueListener(string queueName, IConnection connection)
        {
            _model = connection.CreateModel();
            _messages = new ConcurrentQueue<Message>();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += HandleBasicDeliver;
            _model.BasicConsume(queueName, true, _basicConsumer);
        }

        private void HandleBasicDeliver(object sender, BasicConsumer.HandleBasicDeliverEventArgs e)
        {
            var messageTypeName = e.Properties.GetHeaderValue(MessageFieldConstants.MessageType);
            var message = (Message)BinarySerialization.FromBytes(messageTypeName, e.Body);
            _messages.Enqueue(message);
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
            //if (_connection != null) _connection.Dispose();
        }
    }
}
