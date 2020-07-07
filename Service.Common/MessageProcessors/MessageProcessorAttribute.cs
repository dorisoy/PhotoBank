using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Service.Common.MessageProcessors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageProcessorAttribute : Attribute
    {
        public Type MessageType { get; set; }
    }
}
