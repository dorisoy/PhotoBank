using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoBank.Broker.Api.Authentication;
using PhotoBank.Broker.Api.SignalR;
using PhotoBank.Logger.Common;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Broker.Api
{
    public class Startup
    {
        public readonly string Host = "Broker";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IAuthenticationManager), typeof(AuthenticationManager));
            var queueManagerFactory = new QueueManagerFactory();
            var queueManager = queueManagerFactory.Make();
            services.AddSingleton(typeof(IQueueManager), queueManager);
            services.AddSingleton(typeof(IMessageLogger), new MessageLogger(queueManager, Host));
            BrokerNotifier.Instance.Init(queueManager);
            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = Int64.MaxValue;
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<BrokerHub>("/hub");
            });
        }
    }
}
