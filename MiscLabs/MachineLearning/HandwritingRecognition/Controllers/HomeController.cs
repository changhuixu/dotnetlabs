using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HandwritingRecognition.Models;
using HandwritingRecognitionML.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ML;

namespace HandwritingRecognition.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;
        private const int SizeOfImage = 32;
        private const int SizeOfArea = 4;

        public HomeController(ILogger<HomeController> logger, PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _logger = logger;
            _predictionEnginePool = predictionEnginePool;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Upload(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return BadRequest(new { prediction = "-", dataset = string.Empty });
            }
            var pixelValues = GetPixelValuesFromImage(base64Image.Replace("data:image/png;base64,", ""));
            var input = new ModelInput { PixelValues = pixelValues.ToArray() };
            var result = _predictionEnginePool.Predict(modelName: HWRModel.Name, example: input);
            _logger.LogInformation($"Number {result.Prediction} is returned.");
            return Ok(new
            {
                prediction = result.Prediction, 
                scores = FormatScores(result.Score), 
                pixelValues = string.Join(",", pixelValues)
            });
        }

        private static List<float> GetPixelValuesFromImage(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image).ToArray();

            // resize the original image and save it as bitmap
            var bitmap = new Bitmap(SizeOfImage, SizeOfImage);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                using var stream = new MemoryStream(imageBytes);
                var png = Image.FromStream(stream);
                g.DrawImage(png, 0, 0, SizeOfImage, SizeOfImage);
            }

            // aggregate pixels in 4X4 area --> 'result' is a list of 64 integers
            var result = new List<float>();
            for (var i = 0; i < SizeOfImage; i += SizeOfArea)
            {
                for (var j = 0; j < SizeOfImage; j += SizeOfArea)
                {
                    var sum = 0;        // 'sum' is in the range of [0,16].
                    for (var k = i; k < i + SizeOfArea; k++)
                    {
                        for (var l = j; l < j + SizeOfArea; l++)
                        {
                            if (bitmap.GetPixel(l, k).Name != "ffffffff") sum++;
                        }
                    }
                    result.Add(sum);
                }
            }

            return result;
        }

        private static string FormatScores(IReadOnlyList<float> scores)
        {
            // order is: 0,7,4,6,2,5,8,1,9,3 (the order of labels appear in the training data set)
            var sb = new StringBuilder();
            sb.Append($"0: {scores[0]:N3}").AppendLine();
            sb.Append($"1: {scores[7]:N3}").AppendLine();
            sb.Append($"2: {scores[4]:N3}").AppendLine();
            sb.Append($"3: {scores[9]:N3}").AppendLine();
            sb.Append($"4: {scores[2]:N3}").AppendLine();
            sb.Append($"5: {scores[5]:N3}").AppendLine();
            sb.Append($"6: {scores[3]:N3}").AppendLine();
            sb.Append($"7: {scores[1]:N3}").AppendLine();
            sb.Append($"8: {scores[6]:N3}").AppendLine();
            sb.Append($"9: {scores[8]:N3}");
            return sb.ToString();
        }
    }
}
