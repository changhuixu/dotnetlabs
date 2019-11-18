using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using StatisticsToolbox.Statistics;

namespace StatisticsToolbox
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var sdCmd = new Command("sd", "Standard Deviation of an array of numbers.")
            {
                new Option(new[] {"--numbers", "-n"}, "numbers.")
                {
                    Argument = new Argument<double[]>(() => new double[] { })
                },
            };
            sdCmd.Handler = CommandHandler.Create<double[]>(numbers =>
            {
                var stdDev = new StandardDeviation(numbers);
                Console.WriteLine($"Population Standard Deviation: {stdDev.Population}");
                Console.WriteLine($"    Sample Standard Deviation: {stdDev.Sample}");
            });

            var avgCmd = new Command("avg", "Arithmetic Average of an array of numbers.")
            {
                new Option(new[] {"--numbers", "-n"}, "numbers.")
                {
                    Argument = new Argument<double[]>(() => new double[] { })
                },
            };
            avgCmd.Handler = CommandHandler.Create<double[]>(numbers =>
            {
                Console.WriteLine($"Arithmetic Average: {numbers.Average()}");
            });

            var rootCommand = new RootCommand
            {
                sdCmd, avgCmd
            };

            rootCommand.Name = "stat";
            rootCommand.Description = "Statistics Toolbox";

            if (args.Length == 0)
            {
                args = new[] { "-h" };
            }
            else if (args.Length == 1)
            {
                if (rootCommand.Children.SelectMany(x => x.Aliases).Contains(args[0]))
                {
                    args = args.Append("-h").ToArray();
                }
            }
            await rootCommand.InvokeAsync(args);
        }
    }
}
