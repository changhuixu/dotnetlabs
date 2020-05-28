using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderApi.RabbitMQ;
using RabbitMQ.Client;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitHostName ?? "localhost",
                Port = 5672,
                UserName = "ops0",
                Password = "ops0"
            };
            var rabbitMqConnection = connectionFactory.CreateConnection();
            services.AddSingleton(rabbitMqConnection);
            services.AddSingleton<RabbitMQClient>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrdersAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Orders API V1");
                c.DocumentTitle = "Orders API";
                c.RoutePrefix = string.Empty;
            });
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            hostApplicationLifetime.ApplicationStarted.Register(() => { });
            hostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                var rabbitMqClient = app.ApplicationServices.GetRequiredService<RabbitMQClient>();
                rabbitMqClient.CloseConnection();
            });
        }
    }
}
