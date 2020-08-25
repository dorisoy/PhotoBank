using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace PhotoBank.QueueLogic.Utils
{
    public static class QueueLogicConfig
    {
        private static IConfigurationRoot _root;

        static QueueLogicConfig()
        {
            _root = new ConfigurationBuilder().AddJsonFile("QueueLogicConfig.json", optional: true, reloadOnChange: true).Build();
        }

        public static string QueueManagerTypeName
        {
            get
            {
                return _root.GetSection("queueManagerTypeName").Value;
            }
        }
    }
}
