using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureServices((hostContext, services) =>
                {
                    var queueManager = new QueueManager();
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
                    services.AddSingleton(typeof(IMessageProcessorFactory), processorFactory);

                    services.AddHostedService<PhotoWorker>();
                });
    }
}
