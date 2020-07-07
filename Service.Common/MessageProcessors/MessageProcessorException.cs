using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Service.Common.MessageProcessors
{
    public class MessageProcessorException : Exception
    {
        public MessageProcessorException(string message) : base(message)
        {
        }
    }
}
