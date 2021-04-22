using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PhotoBank.QueueLogic.Contracts;

namespace PhotoBank.Broker.Api.SignalR
{
    public class OutputMessageConvertersCollection
    {
        private readonly Dictionary<Type, IOutputMessageConverter> _converters;

        public OutputMessageConvertersCollection()
        {
            var converterTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && typeof(IOutputMessageConverter).IsAssignableFrom(type)).ToList();
            _converters = converterTypes.Select(Activator.CreateInstance).Cast<IOutputMessageConverter>().ToDictionary(k => k.MessageType, v => v);
        }

        public object ToResponse(OutputMessage outputMessage)
        {
            var converter = _converters[outputMessage.GetType()];
            var response = converter.ToResponse(outputMessage);
            return response;
        }
    }
}
