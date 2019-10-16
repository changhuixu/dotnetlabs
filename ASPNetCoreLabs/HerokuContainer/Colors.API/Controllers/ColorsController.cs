using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Colors.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Colors.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly ILogger<ColorsController> _logger;

        public ColorsController(ILogger<ColorsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Computes the contrast ratio between two colors.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/contrast-ratio
        ///     {
        ///        "fc": "00FF00",
        ///        "bc": "FFFFFF"
        ///     }
        ///
        /// Sample response:
        ///
        ///     {
        ///         "foregroundColor": "ff00ff00",
        ///         "backgroundColor": "ffffffff",
        ///         "ratio": 1.372
        ///     }
        ///         
        /// </remarks>
        /// <param name="fc">Foreground Color: a HEX number</param>
        /// <param name="bc">Background Color: a HEX number</param>
        /// <response code="200">Returns the ColorContrast object</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response> 
        [HttpGet("contrast-ratio")]
        [ProducesResponseType(typeof(ContrastRatio), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        public ActionResult<ContrastRatio> GetContrastRatio(string fc, string bc)
        {
            if (string.IsNullOrWhiteSpace(fc))
            {
                ModelState.AddModelError(nameof(fc), "Foreground color is missing. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(bc))
            {
                ModelState.AddModelError(nameof(bc), "Background color is missing. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            if (!int.TryParse(fc, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(nameof(fc), $"The color [{fc}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }
            if (!int.TryParse(bc, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(nameof(bc), $"The color [{bc}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Checking the contrast ration between Foreground Color [{fc}] and Background Color [{bc}].");
            var fColor = ColorTranslator.FromHtml("#" + fc);
            var bColor = ColorTranslator.FromHtml("#" + bc);
            return new ContrastRatio
            {
                ForegroundColor = fColor.Name,
                BackgroundColor = bColor.Name,
                Ratio = ColorServices.GetContrastRatio(fColor, bColor)
            };
        }

        public class ContrastRatio
        {
            public string ForegroundColor { get; set; }
            public string BackgroundColor { get; set; }
            public double Ratio { get; set; }
        }
    }
}
