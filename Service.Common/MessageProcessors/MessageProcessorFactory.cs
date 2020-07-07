using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Service.Common.MessageProcessors
{
    public interface IMessageProcessorFactory
    {
        void Add(Type messageProcessorType);

        MessageProcessor MakeProcessorFor(Message message);
    }

    public class MessageProcessorFactory : IMessageProcessorFactory
    {
        private readonly MessageProcessorContext _context;
        private readonly Dictionary<Type, Type> _messageProcessors;

        public MessageProcessorFactory(MessageProcessorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _messageProcessors = new Dictionary<Type, Type>();
        }

        public void Add(Type messageProcessorType)
        {
            var attr = messageProcessorType.GetCustomAttribute<MessageProcessorAttribute>();
            _messageProcessors.Add(attr.MessageType, messageProcessorType);
        }

        public MessageProcessor MakeProcessorFor(Message message)
        {
            var messageProcessorType = _messageProcessors[message.GetType()];
            var messageProcessor = (MessageProcessor)Activator.CreateInstance(messageProcessorType);
            messageProcessor.SetContext(_context);
            messageProcessor.SetMessage(message);

            return messageProcessor;
        }
    }
}
