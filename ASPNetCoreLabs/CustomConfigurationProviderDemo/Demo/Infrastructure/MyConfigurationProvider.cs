using System;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace Demo.Infrastructure
{
    public class MyConfigurationProvider : ConfigurationProvider
    {
        private readonly MyConfigurationOptions _options;

        public MyConfigurationProvider(MyConfigurationOptions options)
        {
            _options = options;
        }

        public override void Load()
        {
            using var conn = new SQLiteConnection(_options.ConnectionString);
            conn.Open();
            using var cmd = new SQLiteCommand(_options.Query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Data.Add(reader.GetString(0), reader.GetString(1));
            }
        }
    }

    public class MyConfigurationOptions
    {
        public string ConnectionString { get; set; }
        public string Query { get; set; }
    }

    public class MyConfigurationSource : IConfigurationSource
    {
        private readonly MyConfigurationOptions _options;

        public MyConfigurationSource(MyConfigurationOptions options)
        {
            _options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyConfigurationProvider(_options);
        }
    }

    public static class MyConfigurationExtensions
    {
        public static IConfigurationBuilder AddMyConfiguration(this IConfigurationBuilder configuration, Action<MyConfigurationOptions> options)
        {
            var myConfigsOptions = new MyConfigurationOptions();
            options(myConfigsOptions);
            configuration.Add(new MyConfigurationSource(myConfigsOptions));
            return configuration;
        }
    }
}
