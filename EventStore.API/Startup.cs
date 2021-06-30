using EventStore.API.Model;
using EventStore.API.Model.Response;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetAssembly(GetType()));
            services.AddSingleton<IEventStore, MartenEventStore>(_ => new MartenEventStore(Configuration.GetConnectionString("default")));

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AuthenticationSecret")?.Value ?? "Very very long secret key to authenticate");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    SaveSigninToken = true
                };
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventStore.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
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

        #endregion Public Methods
    }
}