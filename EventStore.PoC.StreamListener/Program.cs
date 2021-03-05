using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Baseline.Dates;
using EventStore.PoC.Domain.Entity;
using EventStore.PoC.Store.EventStore.Impl.MartenDb;
using EventStore.PoC.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Configuration;



namespace EventStore.PoC.StreamListener
{
    class Program
    {

        private static IConfigurationRoot _ConfigurationRoot
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true);
                return builder.Build();
            }
        }
        static async Task Main(string[] args)
        {

            var eventStore = new MartenEventStore(_ConfigurationRoot.GetConnectionString("marten"));


            eventStore.DocumentStore.Options.Events.InlineProjections.Add(new ContentAggregation());

            using (var session = eventStore.DocumentStore.QuerySession())
            {
                var contents = await session.Query<Content>().ToListAsync();


            }



            Console.ReadKey();

            Console.WriteLine("Hello World!");
        }
    }
}
