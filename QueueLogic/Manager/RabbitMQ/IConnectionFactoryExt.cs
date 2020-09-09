using System;
using System.Threading;
using RabbitMQ.Client;

namespace PhotoBank.QueueLogic.Manager.RabbitMQ
{
    public static class IConnectionFactoryExt
    {
        public static IConnection TryCreateConnection(this IConnectionFactory connectionFactory)
        {
            var attemptTimeout = TimeSpan.FromSeconds(10);
            int attemptsCount = 3;
            Exception lastException = null;
            for (int attempt = 0; attempt < attemptsCount; attempt++)
            {
                try
                {
                    return connectionFactory.CreateConnection();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Thread.Sleep(attemptTimeout);
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }
            else
            {
                return null;
            }
        }
    }
}
