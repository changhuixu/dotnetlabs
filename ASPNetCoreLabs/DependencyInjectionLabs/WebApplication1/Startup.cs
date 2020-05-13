using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Test2>();
            services.AddHttpClient<Test1>();
        }

        public class Test1
        {
            private readonly HttpClient _client;
            private readonly Test2 _test2;

            public Test1(HttpClient client, IServiceProvider serviceProvider)
            {
                _client = client;
                using (var scope = serviceProvider.CreateScope())
                {
                    _test2 = scope.ServiceProvider.GetRequiredService<Test2>();
                }
            }
        }

        public class Test2 { }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var a = context.RequestServices.GetRequiredService<Test1>();
                await context.Response.WriteAsync("Hello World22!");
            });
        }
    }
}
