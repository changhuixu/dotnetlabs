using System.Drawing;
using System.Globalization;
using Colors.API.Services;
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

        // http://localhost:5000/api/colors/contrast-ratio?fc=00FF00&bc=FFFFFF  ---> 1.372
        [HttpGet("contrast-ratio")]
        public ActionResult<double> GetContrastRatio(string fc, string bc)
        {

            if (string.IsNullOrWhiteSpace(fc))
            {
                ModelState.AddModelError(fc, "Foreground color is missing. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(bc))
            {
                ModelState.AddModelError(bc, "Background color is missing. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            if (!int.TryParse(fc, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(fc, $"The color [{fc}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }
            if (!int.TryParse(bc, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(bc, $"The color [{bc}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Checking the contrast ration between Foreground Color [{fc}] and Background Color [{bc}].");
            return ColorServices.GetContrastRatio(ColorTranslator.FromHtml("#" + fc), ColorTranslator.FromHtml("#" + bc));
        }

    }
}
