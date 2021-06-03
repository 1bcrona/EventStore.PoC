using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.API.MassTransit.Consumer;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventStore.API.MassTransit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddScoped<IEventStore, MartenEventStore>(_ => new MartenEventStore("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=postgres;"));
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddContentCommandConsumer>();
                x.AddRequestClient<AddContentCommand>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configurator =>
                    {
                        configurator.ConfigureEndpoints(provider);
                        configurator.Host("localhost", "/", cfg =>
                        {
                            cfg.Username("guest");
                            cfg.Password("guest");
                        });
                    }
                ));
            });

            services.AddLogging();
            services.AddMassTransitHostedService();

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
