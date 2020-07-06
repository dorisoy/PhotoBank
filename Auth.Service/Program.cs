using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoBank.Auth.Service.Data;
using PhotoBank.DataAccess;
using PhotoBank.QueueLogic.Manager;

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
                InitRepositoryFactory(services, hostContext.Configuration);
                services.AddSingleton(typeof(IQueueManager), typeof(QueueManager));
                services.AddHostedService<AuthWorker>();
            });

        private static void InitRepositoryFactory(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["connectionString"];
            var context = new AuthServiceDBContext(connectionString);
            var factory = new RepositoryFactory();
            factory.Add(typeof(IUserRepository), new UserRepository(context));
            services.AddSingleton(typeof(IRepositoryFactory), factory);
        }
    }
}
