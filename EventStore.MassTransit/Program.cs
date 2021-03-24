using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EventStore.MassTransit
{
    class Program
    {


        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddMassTransit();

            var provider = services.BuildServiceProvider();
            var busControl = provider.GetRequiredService<IBusControl>();

            await busControl.StartAsync();

            Console.ReadKey();

            await busControl.StopAsync();

        }
    }
}
