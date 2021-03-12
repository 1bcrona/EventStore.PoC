using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EventStore.StreamListener
{
    internal class Program
    {
        #region Private Properties

        private static IConfigurationRoot _ConfigurationRoot
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true);
                return builder.Build();
            }
        }

        #endregion Private Properties

        #region Private Methods

        private static IServiceCollection InitializeContainer()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IEventStore, MartenEventStore>(_ =>
                new MartenEventStore(_ConfigurationRoot.GetConnectionString("marten")));
            services.AddSingleton<App>();

            return services;
        }

        private static async Task Main(string[] args)
        {
            var serviceCollection = InitializeContainer();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider?.GetService<App>()?.Run();

            Console.ReadKey();
        }

        #endregion Private Methods
    }
}