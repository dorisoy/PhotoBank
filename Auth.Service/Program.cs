using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.Auth.Contracts;
using PhotoBank.Auth.Service.Data;
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
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                var queueManagerFactory = new QueueManagerFactory();
                var queueManager = queueManagerFactory.Make();
                services.AddSingleton(typeof(IQueueManager), queueManager);

                AuthSettings.RootUserPictures = hostContext.Configuration["rootUserPictures"];

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
                processorFactory.AddFromAssembly(Assembly.GetExecutingAssembly());
                services.AddSingleton(typeof(IMessageProcessorFactory), processorFactory);

                var obsoleteUserPictureRemover = new ObsoleteUserPictureRemover(repositoryFactory);
                obsoleteUserPictureRemover.Remove();

                services.AddHostedService<AuthWorker>();
            });
    }
}
