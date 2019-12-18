using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;

namespace Colors.API
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
            services.AddSwaggerDocument(c =>
            {
                c.PostProcess = doc =>
                {
                    doc.Info.Version = @"v1";
                    doc.Info.Title = @"Colors API";
                    doc.Info.Description = @"A simple example ASP.NET Core Web API";
                    doc.Info.Contact = new OpenApiContact
                    {
                        Name = @"GitHub Repository",
                        Email = string.Empty,
                        Url = @"https://github.com/changhuixu/dotnetlabs/tree/master/ASPNetCoreLabs/HerokuContainer"
                    };
                };
            });

            services.AddHttpsRedirection(options => { options.HttpsPort = 443; });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                           ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            app.UseForwardedHeaders();
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DYNO")))
            {
                app.UseHttpsRedirection();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(c =>
            {
                c.DocExpansion = @"list";
                c.Path = string.Empty;   // To serve the Swagger UI at the app root (http://localhost:<port>/), set the RoutePrefix property to an empty string.
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
