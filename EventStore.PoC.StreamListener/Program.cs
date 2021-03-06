using Baseline.Dates;
using EventStore.PoC.Store.EventStore.Impl.MartenDb;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EventStore.PoC.StreamListener
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
            store.Events.AsyncProjections.AggregateStreamsWith<ContentAggregation>();
            store.Events.ProjectView<ContentAggregation, Guid>();

            var theSession = store.LightweightSession();

            var settings = new DaemonSettings
            {
                LeadingEdgeBuffer = 0.Seconds()
            };

            using (var daemon = store.BuildProjectionDaemon(new[] { typeof(ContentAggregation) }, settings: settings))
            {
                await daemon.RebuildAll();
                daemon.StartAll();
                await daemon.WaitForNonStaleResults();
                await daemon.StopAll();
            }

            var contents = await theSession.Query<ContentAggregation>().ToListAsync();
            foreach (var content in contents)
            {
                Console.WriteLine(content.Id);
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        #endregion Private Methods
    }
}