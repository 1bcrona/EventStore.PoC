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
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service Started");
            RunProjections();
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

        #region Private Methods

        private async void RunProjections()
        {
            await _EventStore.AddProjection(new CustomerProjection());
            await _EventStore.AddProjection(new ProductProjection());
            await _EventStore.AddProjection(new OrderProjection());
            await _EventStore.StartProjectionDaemon();
        }

        #endregion Private Methods
    }
}