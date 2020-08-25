using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoBank.Auth.Service.Data;
using PhotoBank.Auth.Service.MessageProcessors;
using PhotoBank.DataAccess;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Auth.Service
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
                var contextFactory = new AuthServiceDBContextFactory(connectionString);
                var repositoryFactory = new RepositoryFactory();
                repositoryFactory.Add(typeof(IUserRepository), () => new UserRepository(contextFactory));
                repositoryFactory.Add(typeof(ITokenRepository), () => new TokenRepository(contextFactory));
                services.AddSingleton(typeof(IRepositoryFactory), repositoryFactory);

                var processorContext = new MessageProcessorContext
                {
                    QueueManager = queueManager,
                    RepositoryFactory = repositoryFactory
                };
                var processorFactory = new MessageProcessorFactory(processorContext);
                processorFactory.Add(typeof(CreateUserInputMessageProcessor));
                processorFactory.Add(typeof(LoginInputMessageProcessor));
                processorFactory.Add(typeof(CheckTokenInputMessageProcessor));
                services.AddSingleton(typeof(IMessageProcessorFactory), processorFactory);

                services.AddHostedService<AuthWorker>();
            });
    }
}
