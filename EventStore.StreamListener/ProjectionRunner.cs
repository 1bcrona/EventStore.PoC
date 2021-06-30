using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.StreamListener
{
    public class ProjectionRunner : BackgroundService
    {
        #region Private Fields

        private readonly IEventStore _EventStore;

        #endregion Private Fields

        #region Public Constructors

        public ProjectionRunner(IEventStore eventStore)
        {
            _EventStore = eventStore;

            _EventStore.AddProjection(new CustomerProjection());
            _EventStore.AddProjection(new ProductProjection());
            _EventStore.AddProjection(new OrderProjection());
            _EventStore.StartProjectionDaemon();
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service Started");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await base.StopAsync(cancellationToken);
            }
        }

        #endregion Public Methods

        #region Protected Methods

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

        #endregion Protected Methods
    }
}