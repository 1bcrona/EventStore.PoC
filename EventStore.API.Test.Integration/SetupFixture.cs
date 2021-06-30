using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventStore.API.Model.Response;
using EventStore.StreamListener;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace EventStore.API.Test.Integration
{
    public class SetupFixture : IDisposable
    {
        public HttpClient _HttpClient { get; }
        public TestServer _TestServer { get; }

        public static App _App = new App();
        public SetupFixture()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _TestServer = new TestServer(new WebHostBuilder().ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile(configPath);
            }).UseStartup<Startup>());

            _HttpClient = _TestServer.CreateClient();
            Task.Run(async () => await _App.Run());

   
        }


        private void ReleaseUnmanagedResources()
        {
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~SetupFixture()
        {
            ReleaseUnmanagedResources();
        }
    }
}
