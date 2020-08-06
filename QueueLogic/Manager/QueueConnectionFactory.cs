using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager
{
    class QueueConnectionFactory
    {
        public static ConnectionFactory MakeConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "vinge",
                Password = "vinge",
            };
        }
    }
}
