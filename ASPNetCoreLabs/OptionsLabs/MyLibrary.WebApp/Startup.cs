using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyLibrary.WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.AddConsole());
            services.AddMyService(options =>
            {
                options.Option1 = "100 push-ups";
                options.Option2 = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var myService = app.ApplicationServices.GetRequiredService<IMyService>();
            //myService.DoWork();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var myService = context.RequestServices.GetRequiredService<IMyService>();
                    myService.DoWork();
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
