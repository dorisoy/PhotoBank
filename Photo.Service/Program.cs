using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.DataAccess;
using PhotoBank.Photo.Service.Data;
using PhotoBank.Photo.Service.MessageProcessors;
using PhotoBank.QueueLogic.Manager;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service
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

                var connectionString = hostContext.Configuration["connectionString"];
                var contextFactory = new PhotoServiceDBContextFactory(connectionString);
                var repositoryFactory = new RepositoryFactory();
                repositoryFactory.Add(typeof(IPhotoRepository), () => new PhotoRepository(contextFactory));
                services.AddSingleton(typeof(IRepositoryFactory), repositoryFactory);

                var processorContext = new MessageProcessorContext
                {
                    QueueManager = queueManager,
                    RepositoryFactory = repositoryFactory
                };
                var processorFactory = new MessageProcessorFactory(processorContext);
                processorFactory.Add(typeof(GetPhotoInputMessageProcessor));
                processorFactory.Add(typeof(GetPhotosInputMessageProcessor));
                processorFactory.Add(typeof(UploadPhotoInputMessageProcessor));
                processorFactory.Add(typeof(DeletePhotoInputMessageProcessor));
                processorFactory.Add(typeof(GetPhotoAdditionalInfoInputMessageProcessor));
                processorFactory.Add(typeof(SetPhotoAdditionalInfoInputMessageProcessor));
                services.AddSingleton(typeof(IMessageProcessorFactory), processorFactory);

                services.AddHostedService<PhotoWorker>();
            });
    }
}
