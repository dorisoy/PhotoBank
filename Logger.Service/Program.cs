using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoBank.DataAccess;
using PhotoBank.Logger.Service.Data;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace Logger.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var queueManagerFactory = new QueueManagerFactory();
                    var queueManager = queueManagerFactory.Make();
                    services.AddSingleton(typeof(IQueueManager), queueManager);

                    var connectionString = hostContext.Configuration["connectionString"];
                    var contextFactory = new LoggerServiceDBContextFactory(connectionString);
                    var repositoryFactory = new RepositoryFactory();
                    repositoryFactory.Add(typeof(ILogRepository), () => new LogRepository(contextFactory));
                    services.AddSingleton(typeof(IRepositoryFactory), repositoryFactory);

                    var processorContext = new MessageProcessorContext
                    {
                        QueueManager = queueManager,
                        RepositoryFactory = repositoryFactory
                    };
                    var processorFactory = new MessageProcessorFactory(processorContext);
                    processorFactory.AddFromAssembly(Assembly.GetExecutingAssembly());
                    services.AddSingleton(typeof(IMessageProcessorFactory), processorFactory);

                    services.AddHostedService<LoggerWorker>();
                });
    }
}
