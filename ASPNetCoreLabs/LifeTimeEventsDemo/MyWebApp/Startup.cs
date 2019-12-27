using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            LogPath = Path.Combine(env.ContentRootPath, @"App_Data", @"app-log.txt");
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        private string LogPath { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            lifetime.ApplicationStarted.Register(OnAppStarted);

            lifetime.ApplicationStopping.Register(OnAppStopping);

            lifetime.ApplicationStopped.Register(OnAppStopped);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        public void OnAppStarted()
        {
            File.AppendAllText(LogPath, $"App started at {DateTime.Now}\r\n");
        }


        public void OnAppStopping()
        {
            File.AppendAllText(LogPath, $"App stopping at {DateTime.Now}\r\n");
        }


        public void OnAppStopped()
        {
            File.AppendAllText(LogPath, $"App stopped at {DateTime.Now}\r\n");
        }
    }
}
