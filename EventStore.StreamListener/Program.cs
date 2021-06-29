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
        private static readonly App _APP = new();

        private static async Task Main(string[] args)
        {
            await _APP.Run();
        }
    }
}