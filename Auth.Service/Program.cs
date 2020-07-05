using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddSingleton(typeof(IRepositoryFactory), typeof(RepositoryFactory));
                    services.AddSingleton(typeof(IQueueManager), typeof(QueueManager));
                    services.AddHostedService<Worker>();
                });
    }
}
