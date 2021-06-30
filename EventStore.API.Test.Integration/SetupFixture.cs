using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Postgres;
using Xunit;

namespace EventStore.API.Test.Integration
{
    public class EnvironmentFixture : IAsyncLifetime
    {
        #region Private Fields

        private DockerEnvironment _Environment;
        private IHost _Host;
        private IHost _ListenerService;

        #endregion Private Fields

        #region Public Constructors

        public EnvironmentFixture()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public HttpClient _HttpClient { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public async Task DisposeAsync()
        {
            _HttpClient?.Dispose();
            _Host?.Dispose();
            _ListenerService?.Dispose();
            await _Environment.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            _Environment = CreateTestEnvironmentBuilder().Build();
            await _Environment.Up();

            var container = _Environment.GetContainer<PostgresContainer>("test-postgres");

            SetConfigurationForTest(container);
            _Host = await CreateHostBuilder().StartAsync();
            _ListenerService = await CreateBackgroundService().StartAsync();
            _HttpClient = _Host.GetTestClient();
            await OnInitialized(container);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual Task OnInitialized(PostgresContainer postgresContainer) => Task.CompletedTask;

        private void SetConfigurationForTest(PostgresContainer container)
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] {
                    new KeyValuePair<string, string>("ConnectionStrings:default", container.GetConnectionString()),
                    new KeyValuePair<string, string>("ConnectionStrings:marten", container.GetConnectionString())
                }).Build();
        }

        #endregion Protected Methods

        #region Private Methods

        public IConfiguration Configuration;

        private IHostBuilder CreateBackgroundService()
        {
            var builder = new HostBuilder()
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                })
                .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddConfiguration(Configuration))
                .ConfigureServices(services =>
                {
                    services.AddScoped<IEventStore, MartenEventStore>(_ => new MartenEventStore(Configuration.GetConnectionString("marten")));
                    services.AddSingleton<App>();
                    services.AddHostedService<ProjectionRunner>();
                });

            return builder;
        }

        private IHostBuilder CreateHostBuilder()
        {
            var builder = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseConfiguration(Configuration)
                        .UseTestServer();
                });

            return builder;
        }

        private IDockerEnvironmentBuilder CreateTestEnvironmentBuilder()
        {
            var ports = new Dictionary<ushort, ushort> { [5432] = 49153 };

            return new DockerEnvironmentBuilder()
                    .SetName("test")
                    .AddPostgresContainer("test-postgres", userName: "postgres", password: "password", ports: ports,
                        reuseContainer: true);
        }

        #endregion Private Methods
    }
}