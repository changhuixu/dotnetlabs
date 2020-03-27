using System;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace Demo.Infrastructure
{
    public class MyConfigurationProvider : ConfigurationProvider
    {
        public MyConfigurationSource Source { get; }

        public MyConfigurationProvider(MyConfigurationSource source)
        {
            Source = source;
        }

        public override void Load()
        {
            using var conn = new SQLiteConnection(Source.ConnectionString);
            conn.Open();
            using var cmd = new SQLiteCommand(Source.Query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Set(reader.GetString(0), reader.GetString(1));
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
        public string ConnectionString { get; set; }
        public string Query { get; set; }

        public MyConfigurationSource(MyConfigurationOptions options)
        {
            ConnectionString = options.ConnectionString;
            Query = options.Query;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyConfigurationProvider(this);
        }
    }

    public static class MyConfigurationExtensions
    {
        public static IConfigurationBuilder AddMyConfiguration(this IConfigurationBuilder configuration, Action<MyConfigurationOptions> options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            var myConfigurationOptions = new MyConfigurationOptions();
            options(myConfigurationOptions);
            configuration.Add(new MyConfigurationSource(myConfigurationOptions));
            return configuration;
        }
    }
}
