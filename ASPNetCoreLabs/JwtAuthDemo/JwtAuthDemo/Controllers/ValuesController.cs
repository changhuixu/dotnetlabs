using System;
using System.Collections.Generic;
using System.Linq;
using JwtAuthDemo.Infrastructure.BasicAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<int> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 3).Select(x => rng.Next(0, 100));
        }

        [HttpGet("jwt")]
        [Authorize]
        public IEnumerable<int> JwtAuth()
        {
            _logger.LogInformation("jwt auth");
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
        }


        [HttpGet("basic")]
        [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
        public IEnumerable<int> BasicAuth()
        {
            _logger.LogInformation("basic auth");
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
        }

        [HttpGet("basic-logout")]
        [BasicAuth]
        public IActionResult BasicAuthLogout()
        {
            _logger.LogInformation("basic auth logout");
            // NOTE: there's no good way to log out basic authentication. This method is a hack.
            HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"My Realm\"";
            return new UnauthorizedResult();
        }
    }
}
