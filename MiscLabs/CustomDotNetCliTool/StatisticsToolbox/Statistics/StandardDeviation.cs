using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsToolbox.Statistics
{
    public class StandardDeviation
    {
        public double Population { get; }
        public double Sample { get; }
        public StandardDeviation(IReadOnlyCollection<double> numbers)
        {
            Population = PopulationStandardDeviation(numbers);
            Sample = SampleStandardDeviation(numbers);
        }
        private static double PopulationStandardDeviation(IReadOnlyCollection<double> numberSet)
        {
            return StdDev(numberSet, numberSet.Count);
        }

        public static double SampleStandardDeviation(IReadOnlyCollection<double> numberSet)
        {
            return StdDev(numberSet, numberSet.Count - 1);
        }
        private static double StdDev(IReadOnlyCollection<double> numberSet, double divisor)
        {
            var mean = numberSet.Average();
            return Math.Sqrt(numberSet.Sum(x => Math.Pow(x - mean, 2)) / divisor);
        }
    }
}
