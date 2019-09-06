using Microsoft.Extensions.Logging;

namespace ClassLibrary
{
    public interface ICalculationService
    {
        int AddTwoPositiveNumbers(int a, int b);
    }

    public class CalculationService : ICalculationService
    {
        private readonly ILogger<CalculationService> _logger;

        public CalculationService(ILogger<CalculationService> logger)
        {
            _logger = logger;
        }

        public int AddTwoPositiveNumbers(int a, int b)
        {
            if (a <= 0 || b <= 0)
            {
                _logger.LogError("Arguments should be both positive.");
                return 0;
            }
            _logger.LogInformation($"Adding {a} and {b}");
            return a + b;
        }
    }
}
