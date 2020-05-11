using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyWebApi
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/html";
                    var content = $"<h2>Hello World from {Environment.MachineName}!</h2>";
                    if (serverAddressesFeature != null)
                    {
                        content += $"<p>Listening on the following addresses: {string.Join(", ", serverAddressesFeature.Addresses)}</p>";
                    }
                    content += $"<p>Request URL: {context.Request.GetDisplayUrl()}<p>";
                    await context.Response.WriteAsync(content);
                });
            });
        }
    }
}
