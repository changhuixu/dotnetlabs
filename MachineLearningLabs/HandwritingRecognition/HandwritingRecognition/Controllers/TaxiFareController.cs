using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using TaxiFareML.Model;

namespace HandwritingRecognition.Controllers
{
    [Route("api/[controller]")]
    public class TaxiFareController : Controller
    {
        private readonly ILogger<TaxiFareController> _logger;
        private readonly PredictionEnginePool<TaxiFareModelInput, TaxiFareModelOutput> _predictionEnginePool;

        public TaxiFareController(ILogger<TaxiFareController> logger, PredictionEnginePool<TaxiFareModelInput, TaxiFareModelOutput> predictionEnginePool)
        {
            _logger = logger;
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpGet("prediction")]
        public IActionResult Predict()
        {
            var input = new TaxiFareModelInput
            {
                VendorId = "VTS",
                RateCode = 1,
                PassengerCount = 1,
                TripTimeInSecs = 1140,
                TripDistance = 3.75f,
                PaymentType = "CSH",
                FareAmount = 0 // To predict. Actual/Observed = 15.5
            };
            var result = _predictionEnginePool.Predict(modelName: TaxiFareModel.Name, example: input);
            _logger.LogInformation($"Predicted fare: {result.FareAmount}, actual fare: 15.5");
            return Ok(result);
        }
    }
}
