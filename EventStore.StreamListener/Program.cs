using Baseline.Dates;
using EventStore.Store.EventStore.Impl.MartenDb;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.ValueObject;
using Marten.Events.Projections;

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

            using (var daemon = store.BuildProjectionDaemon(new[] { typeof(ContentProjection) }, null, settings, new IProjection[] { new ContentProjection() }))
            {
                await daemon.RebuildAll();
                daemon.StartAll();
                await daemon.WaitForNonStaleResults();
                await daemon.StopAll();
            }


            var contents = await theSession.Query<Content>().ToListAsync();
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