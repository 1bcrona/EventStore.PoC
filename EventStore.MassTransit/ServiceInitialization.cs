using System.Linq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Emit;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;

namespace EventStore.MassTransit
{
    public static class ServiceInitialization
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services)
        {

            services.AddScoped<IEventStore, MartenEventStore>(_ => new MartenEventStore("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=postgres;"));
            services.AddMassTransit(config =>
            {

                config.AddConsumers(Assembly.GetExecutingAssembly());
                config.SetKebabCaseEndpointNameFormatter();
                var consumerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(w => !w.IsInterface && typeof(IConsumer).IsAssignableFrom(w));
                
                foreach (var type in consumerTypes)
                {
                    config.AddRequestClient(type);
                }

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host("localhost", "/", configurator =>
                    {
                        configurator.Username("guest");
                        configurator.Password("guest");
                    });

                });


            });





            return services;
        }
    }
}