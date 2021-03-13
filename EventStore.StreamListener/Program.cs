using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline.Dates;
using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection;
using EventStore.StreamListener.Projection.Marten;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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


        private static void Main(string[] args)
        {

            var serviceCollection = InitializeContainer();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider?.GetService<App>()?.Run();

            Console.ReadKey();
        }

        #endregion Private Methods
    }
}