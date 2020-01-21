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
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<int> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(x => rng.Next(0, 100));
        }

        [HttpGet("basic")]
        [BasicAuth("11111111111111111")]
        public IEnumerable<int> Get2()
        {
            _logger.LogInformation("basic auth");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(x => rng.Next(0, 100));
        }
    }
}
