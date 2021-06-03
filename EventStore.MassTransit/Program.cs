using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Internals.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EventStore.MassTransit
{
    class Program
    {


        static async Task Main(string[] args)
        {

            Console.WriteLine("Starting MassTransit.");

            var services = new ServiceCollection();

            services.AddMassTransit();

            var provider = services.BuildServiceProvider();
            var busControl = provider.GetRequiredService<IBusControl>();

            await busControl.StartAsync();

            //await busControl.Send(new AddContentCommand() { Title = "Mass", Url = "Transit" });
            var x = await busControl.GetSendEndpoint(new Uri("queue:add-content-command"));
            await busControl.Send(new AddContentCommand() { Title = "Mass", Url = "Transit" });
            await x.Send(new AddContentCommand() { Title = "Mass", Url = "Transit" });


            Console.WriteLine("Started. Press any key to exit");
            Console.ReadKey();

            await busControl.StopAsync();



        }
    }
}
