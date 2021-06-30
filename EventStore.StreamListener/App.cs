using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EventStore.StreamListener.Projection.Marten;

namespace EventStore.StreamListener
{
    public class App
    {
        #region Public Properties

        public static IConfiguration Configuration { get; set; }

        #endregion Public Properties

        #region Public Methods

        public async Task Run()
        {
            var environment = Environment.GetEnvironmentVariable("EventStore_Environment") ?? "Production";
            var host = new HostBuilder()
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile(environment == "Production" ? "appsettings.json" : $"appsettings.{environment}.json", true, true);
                    builder.AddEnvironmentVariables("EventStore_");
                    Configuration = builder.Build();
                })
                .ConfigureServices(InitializeContainer)
                .UseEnvironment(environment)
                .Build();


      


            await host.RunAsync();
        }

        #endregion Public Methods

        #region Private Methods

        private void InitializeContainer(IServiceCollection services)
        {
            services.AddScoped<IEventStore, MartenEventStore>(_ =>
                new MartenEventStore(Configuration.GetConnectionString("marten")));
            services.AddSingleton<App>();



            services.AddHostedService<ProjectionRunner>();
        }

        #endregion Private Methods
    }
}