﻿using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Utils;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    class QueueMessageListener<TMessage> : IQueueMessageListener<TMessage> where TMessage : Message
    {
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly BasicConsumer _basicConsumer;
        private TMessage _message;
        private readonly string _messageGuid;
        private bool _isDisposed;

        public ILogger Logger { get; set; }

        public QueueMessageListener(string queueName, string messageGuid, ConnectionFactory factory)
        {
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();
            _basicConsumer = new BasicConsumer();
            _basicConsumer.OnHandleBasicDeliver += OnHandleBasicDeliver;
            _model.BasicConsume(queueName, false, _basicConsumer);
            _messageGuid = messageGuid;
            _isDisposed = false;
        }

        private void OnHandleBasicDeliver(object sender, BasicConsumer.HandleBasicDeliverEventArgs e)
        {
            var messageContainerGuid = e.Properties.GetHeaderValue(MessageFieldConstants.MessageGuid);
            Logger.LogInformation(String.Format("QueueMessageListener {0}. Wait message {1}. Recieve: {2}", GetHashCode(), _messageGuid, messageContainerGuid));
            if (messageContainerGuid == _messageGuid)
            {
                _model.BasicAck(e.DeliveryTag, false); // отметка, что сообщение получено
                var messageTypeName = e.Properties.GetHeaderValue(MessageFieldConstants.MessageType);
                _message = (TMessage)BinarySerialization.FromBytes(messageTypeName, e.Body);
                _basicConsumer.OnHandleBasicDeliver -= OnHandleBasicDeliver;
            }
        }

        public TMessage WaitForMessage()
        {
            if (_isDisposed) return default(TMessage);
            while (_message == null) Thread.Sleep(1000);
            return _message;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_model != null) _model.Dispose();
                if (_connection != null) _connection.Dispose();
                _isDisposed = true;
            }
        }
    }
}