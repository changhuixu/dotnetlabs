using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SqlConnect
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option(
                    new[] { "--server-name", "-s"},
                    "Database server name.")
                {
                    Argument = new Argument<string>(() => ".")
                },
                new Option(
                    new[] { "--database-name", "-d"},
                    "The name of the database.")
                {
                    Argument = new Argument<string>(() => string.Empty)
                },
                new Option(
                    new []{"--integrated-security", "--trusted-connection", "-i"},
                    "When false, User ID and Password are specified in the connection. When true, the current Windows account credentials are used for authentication.")
                {
                    Argument = new Argument<bool>()
                },
                new Option(
                    new[] {"--user-id", "-u"},
                    "[Optional] The user name for database connection."
                )
                {
                    Argument = new Argument<string>(() =>string.Empty)
                },
                new Option(
                    new[] {"--password", "-p"},
                    "[Optional] The password for database connection."
                )
                {
                    Argument = new Argument<string>(() =>string.Empty)
                },
            };

            rootCommand.Description = "Check SQL database connection.";

            rootCommand.Handler = CommandHandler.Create<string, string, bool, string, string>(
                (serverName, databaseName, integratedSecurity, userId, password) =>
                {
                    var connectionString =
                        BuildConnectionString(serverName, databaseName, integratedSecurity, userId, password);
                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        return;
                    }

                    Console.WriteLine("Opening SQL connection");

                    using var conn = new SqlConnection(connectionString);
                    try
                    {
                        conn.Open();
                        Console.WriteLine($"Connection State: {conn.State}");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        conn.Close();
                        Console.WriteLine("SQL connection closed.");
                        Console.WriteLine();
                    }
                });

            if (args.Length == 0)
            {
                args = new[] { "-h" };
            }
            await rootCommand.InvokeAsync(args);
        }

        private static string BuildConnectionString(string serverName, string databaseName, bool integratedSecurity, string userId, string password)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                Console.WriteLine("Server Name is required.");
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                Console.WriteLine("Database Name is required");
                return string.Empty;
            }

            if (integratedSecurity)
            {
                Console.WriteLine("Use Integrated Security.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("UserID and Password are required");
                    return string.Empty;
                }
            }

            var builder = new SqlConnectionStringBuilder { DataSource = serverName, InitialCatalog = databaseName };
            if (integratedSecurity)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = userId;
                builder.Password = password;
            }
            return builder.ConnectionString;
        }
    }
}
