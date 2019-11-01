using System;
using Microsoft.Extensions.DependencyInjection;

namespace MyLibrary
{
    public static class MyServiceExtensions
    {
        public static IServiceCollection AddMyService(this IServiceCollection serviceCollection, Action<MyServiceOptions> options)
        {
            serviceCollection.AddScoped<IMyService, MyService>();
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide options for MyService.");
            }
            serviceCollection.Configure(options);
            return serviceCollection;
        }
    }
}
