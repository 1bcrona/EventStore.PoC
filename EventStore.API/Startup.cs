using System;
using System.Collections.Generic;
using System.Net;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using EventStore.API.Model;
using EventStore.API.Model.Response;
using EventStore.API.Model.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace EventStore.API
{
    public class Startup
    {
        #region Public Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Public Constructors

        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion Public Properties

        #region Public Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var response = new BaseHttpServiceResponse<object>();
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    switch (exception)
                    {
                        case ApiException ex:
                            context.Response.ContentType = "application/json";
                            response.Error = new ErrorModel
                            {
                                ErrorMessage = ex.ErrorMessage,
                                ErrorCode = ex.ErrorCode
                            };

                            context.Response.StatusCode = (int)ex.StatusCode;
                            break;

                        default:
                            response.Error = new ErrorModel
                            {
                                ErrorMessage = "Unknown exception occured",
                                ErrorCode = "UNKNWN"
                            };
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                });
            });
            ConfigureSwagger(app);

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


        }

        private void ConfigureSwagger(IApplicationBuilder appBuilder)
        {
            var basePath = "/";

            appBuilder.UseSwagger(config =>
            {
                config.RouteTemplate = "swagger/{documentName}/swagger.json";
                config.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = $"{basePath}" } };
                });
            });

            appBuilder.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint($"{basePath}swagger/v1/swagger.json", "Eventstore.Api v1");
            });
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetAssembly(GetType()));
            services.AddSingleton<IEventStore, MartenEventStore>(_ => new MartenEventStore(Configuration.GetConnectionString("default")));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eventstore.API", Version = "v1" });
            });

        }

        #endregion Public Methods
    }
}