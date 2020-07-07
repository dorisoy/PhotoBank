using System;
using System.Reflection;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Service.Common.MessageProcessors
{
    public abstract class MessageProcessor
    {
        private Message _message;
        protected MessageProcessorContext _context;

        public void SetMessage(Message message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));
            _message = message;
        }

        public void SetContext(MessageProcessorContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        protected TMessage GetMessageAs<TMessage>() where TMessage : Message
        {
            var attr = GetType().GetCustomAttribute<MessageProcessorAttribute>();
            if (attr.MessageType == typeof(TMessage))
            {
                return (TMessage)_message;
            }
            else
            {
                throw new MessageProcessorException("Invalid message cast");
            }
        }

        public abstract void Execute();
    }
}
