using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace EventStore.StreamListener
{
    public class ProjectionRunner : BackgroundService
    {
        private readonly IEventStore _EventStore;

        public ProjectionRunner(IEventStore eventStore)
        {
            _EventStore = eventStore;
        }


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service Started");
            RunProjections();
            await base.StartAsync(cancellationToken);
        }

        private async void RunProjections()
        {
            await _EventStore.AddProjection(new CustomerProjection());
            await _EventStore.AddProjection(new ProductProjection());
            await _EventStore.AddProjection(new OrderProjection());
            await _EventStore.StartProjectionDaemon();

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await base.StopAsync(cancellationToken);
            }
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Stopping");
            }
        }
    }
}
