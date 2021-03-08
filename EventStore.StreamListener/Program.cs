using Baseline.Dates;
using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.StreamListener.Projection;
using Marten;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Configuration;
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

        private static async Task Main(string[] args)
        {
            Marten.DocumentStore store = new MartenEventStore(_ConfigurationRoot.GetConnectionString("marten")).DocumentStore;
            var theSession = store.LightweightSession();

            var settings = new DaemonSettings
            {
                LeadingEdgeBuffer = 0.Seconds()
            };

            var daemon = store.BuildProjectionDaemon(
                new[] { typeof(ContentProjection), typeof(UserProjection), typeof(PlayedContentProjection) }, null,
                settings,
                new IProjection[] { new PlayedContentProjection(), new ContentProjection(), new UserProjection(), });
            daemon.StartAll();
            await daemon.WaitForNonStaleResults();

            var contents = await theSession.Query<Content>().ToListAsync();

            Console.WriteLine("Contents");
            foreach (var content in contents)
            {
                Console.WriteLine(content.Id);
            }

            Console.WriteLine("Users");

            var users = await theSession.Query<User>().ToListAsync();
            foreach (var user in users)
            {
                Console.WriteLine(user.Id);
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        #endregion Private Methods
    }
}