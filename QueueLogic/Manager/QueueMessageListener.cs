using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager
{
    class QueueMessageListener<TMessage> : IQueueMessageListener<TMessage> where TMessage : Message
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly BasicConsumer _basicConsumer;
        private TMessage _message;
        private readonly string _messageGuid;
        private bool _disposed;

        public QueueMessageListener(string queueName, string messageGuid, ConnectionFactory factory)
        {
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += OnHandleBasicDeliver;
            _model.BasicConsume(queueName, false, _basicConsumer);
            _messageGuid = messageGuid;
            _disposed = false;
        }

        public QueueMessageListener(string queueName, string messageGuid, IConnection connection)
        {
            _model = connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += OnHandleBasicDeliver;
            _model.BasicConsume(queueName, false, _basicConsumer);
            _messageGuid = messageGuid;
            _disposed = false;
        }

        private void OnHandleBasicDeliver(object sender, BasicConsumer.HandleBasicDeliverEventArgs e)
        {
            var messageContainerGuid = e.Properties.GetHeaderValue(MessageFieldConstants.MessageGuid);
            if (messageContainerGuid == _messageGuid)
            {
                _model.BasicAck(e.DeliveryTag, false); // отметка, что сообщение получено
                var messageTypeName = e.Properties.GetHeaderValue(MessageFieldConstants.MessageType);
                _message = (TMessage)BinarySerialization.FromBytes(messageTypeName, e.Body);
            //    Dispose();
                _basicConsumer.OnHandleBasicDeliver -= OnHandleBasicDeliver;
            }
        }

        public TMessage WaitForMessage()
        {
            if (_disposed) return default(TMessage);
            while (_message == null) ;
            return _message;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_model != null) _model.Dispose();
            //    if (_connection != null) _connection.Dispose();
                _disposed = true;
            }
        }
    }
}
