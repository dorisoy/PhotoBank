using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    static class QueueConnectionFactory
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
