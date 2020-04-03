using System.Drawing;
using System.Globalization;
using Colors.API.Models;
using Colors.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Colors.API.Controllers
{
    /// <summary>
    /// A set of APIs to convert colors and compute color metrics
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly ILogger<ColorsController> _logger;

        public ColorsController(ILogger<ColorsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the color object from a HEX color format.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/00FF00/argb
        ///
        /// Sample response:
        ///
        ///     {
        ///         "r": 0,
        ///         "g": 255,
        ///         "b": 0,
        ///         "a": 255,
        ///         "isKnownColor": false,
        ///         "isEmpty": false,
        ///         "isNamedColor": false,
        ///         "isSystemColor": false,
        ///         "name": "ff00ff00"
        ///     }
        ///
        /// </remarks>
        /// <param name="color"><strong>Color</strong>: a HEX number. <em>Example: 00FF00</em></param>
        /// <response code="200">Returns the color object.</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("{color}/argb")]
        [ProducesResponseType(typeof(Color), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Color> GetArgb(string color)
        {
            if (!int.TryParse(color, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(nameof(color), $"The color [{color}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            return ColorTranslator.FromHtml("#" + color);
        }


        /// <summary>
        /// Get the luminance of a color.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/00FF00/luminance
        ///
        /// Sample response:
        ///
        ///     0.7152
        ///
        /// </remarks>
        /// <param name="color">Color: a HEX number</param>
        /// <response code="200">Returns the luminance value of a color.</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("{color}/luminance")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<double> GetLuminance(string color)
        {
            if (!int.TryParse(color, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(nameof(color), $"The color [{color}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            return ColorServices.GetLuminance(ColorTranslator.FromHtml("#" + color));
        }


        /// <summary>
        /// Get the brightness of a color.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/00FF00/brightness
        ///
        /// Sample response:
        ///
        ///     149.685
        ///
        /// </remarks>
        /// <param name="color">Color: a HEX number</param>
        /// <response code="200">Returns the brightness value of a color.</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("{color}/brightness")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<double> GetBrightness(string color)
        {
            if (!int.TryParse(color, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
            {
                ModelState.AddModelError(nameof(color), $"The color [{color}] is invalid. This API endpoint accepts a HEX number as a color.");
                return BadRequest(ModelState);
            }

            return ColorServices.GetBrightness(ColorTranslator.FromHtml("#" + color));
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
        ///        {
        ///            "color1": {
        ///                "r": 0,
        ///                "g": 255,
        ///                "b": 0,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ff00ff00"
        ///            },
        ///            "color2": {
        ///                "r": 255,
        ///                "g": 255,
        ///                "b": 255,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ffffffff"
        ///            },
        ///            "ratio": 1.372
        ///        }
        ///
        /// </remarks>
        /// <param name="fc">Foreground Color: a HEX number</param>
        /// <param name="bc">Background Color: a HEX number</param>
        /// <response code="200">
        /// Returns the ColorContrastRatio object
        ///
        /// The contrast ratio is calculated based on [this formula](https://www.w3.org/TR/AERT/#color-contrast)
        /// </response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("contrast-ratio")]
        [ProducesResponseType(typeof(ColorContrastRatio), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ColorContrastRatio> GetContrastRatio(string fc, string bc)
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

            _logger.LogInformation($"Checking the Contrast Ratio between Foreground Color [{fc}] and Background Color [{bc}].");
            var fColor = ColorTranslator.FromHtml("#" + fc);
            var bColor = ColorTranslator.FromHtml("#" + bc);
            return new ColorContrastRatio(fColor, bColor);
        }


        /// <summary>
        /// Computes the Brightness Difference between two colors.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/brightness-difference
        ///     {
        ///        "fc": "00FF00",
        ///        "bc": "FFFFFF"
        ///     }
        ///
        /// Sample response:
        ///
        ///        {
        ///            "color1": {
        ///                "r": 0,
        ///                "g": 255,
        ///                "b": 0,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ff00ff00"
        ///            },
        ///            "color2": {
        ///                "r": 255,
        ///                "g": 255,
        ///                "b": 255,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ffffffff"
        ///            },
        ///            "diff": 105.315,
        ///            "acceptable": false
        ///        }
        ///
        /// </remarks>
        /// <param name="fc">Foreground Color: a HEX number</param>
        /// <param name="bc">Background Color: a HEX number</param>
        /// <response code="200">Returns the ColorBrightnessDifference object</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("brightness-difference")]
        [ProducesResponseType(typeof(ColorBrightnessDifference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ColorBrightnessDifference> GetBrightnessDifference(string fc, string bc)
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

            _logger.LogInformation($"Checking the ColorBrightnessDifference between Foreground Color [{fc}] and Background Color [{bc}].");
            var fColor = ColorTranslator.FromHtml("#" + fc);
            var bColor = ColorTranslator.FromHtml("#" + bc);
            return new ColorBrightnessDifference(fColor, bColor);
        }


        /// <summary>
        /// Computes the Color Difference between two colors.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/colors/color-difference
        ///     {
        ///        "fc": "00FF00",
        ///        "bc": "FFFFFF"
        ///     }
        ///
        /// Sample response:
        ///
        ///        {
        ///            "color1": {
        ///                "r": 0,
        ///                "g": 255,
        ///                "b": 0,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ff00ff00"
        ///            },
        ///            "color2": {
        ///                "r": 255,
        ///                "g": 255,
        ///                "b": 255,
        ///                "a": 255,
        ///                "isKnownColor": false,
        ///                "isEmpty": false,
        ///                "isNamedColor": false,
        ///                "isSystemColor": false,
        ///                "name": "ffffffff"
        ///            },
        ///            "diff": 510,
        ///            "acceptable": true
        ///        }
        ///
        /// </remarks>
        /// <param name="fc">Foreground Color: a HEX number</param>
        /// <param name="bc">Background Color: a HEX number</param>
        /// <response code="200">Returns the ColorBrightnessDifference object</response>
        /// <response code="400">If any color string in the query parameters is invalid.</response>
        [HttpGet("color-difference")]
        [ProducesResponseType(typeof(ColorDifference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ColorDifference> GetColorDifference(string fc, string bc)
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

            _logger.LogInformation($"Checking the ColorDifference between Foreground Color [{fc}] and Background Color [{bc}].");
            var fColor = ColorTranslator.FromHtml("#" + fc);
            var bColor = ColorTranslator.FromHtml("#" + bc);
            return new ColorDifference(fColor, bColor);
        }
    }
}
