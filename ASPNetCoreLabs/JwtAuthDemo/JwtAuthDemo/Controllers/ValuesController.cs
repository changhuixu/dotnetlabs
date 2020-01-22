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
            return Enumerable.Range(1, 5).Select(x => rng.Next(0, 100));
        }

        [HttpGet("jwt")]
        [Authorize]
        public IEnumerable<int> JwtAuth()
        {
            _logger.LogInformation("jwt auth");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(x => rng.Next(0, 100));
        }


        [HttpGet("basic")]
        [BasicAuth]
        // [BasicAuth("my-realm")] --> You can optionally provide a specific realm.
        public IEnumerable<int> BasicAuth()
        {
            _logger.LogInformation("basic auth");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(x => rng.Next(0, 100));
        }
    }
}
