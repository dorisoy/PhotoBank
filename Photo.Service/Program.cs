using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoBank.DataAccess;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Manager;

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
                    InitRepositoryFactory(services, hostContext.Configuration);
                    services.AddSingleton(typeof(IQueueManager), typeof(QueueManager));
                    services.AddHostedService<PhotoWorker>();
                });

        private static void InitRepositoryFactory(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["connectionString"];
            var context = new PhotoServiceDBContext(connectionString);
            var factory = new RepositoryFactory();
            factory.Add(typeof(IPhotoRepository), new PhotoRepository(context));
            services.AddSingleton(typeof(IRepositoryFactory), factory);
        }
    }
}
