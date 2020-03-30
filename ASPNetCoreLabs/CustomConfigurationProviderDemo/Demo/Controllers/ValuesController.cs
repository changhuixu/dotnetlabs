using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly Dictionary<string, string> _configs;
        private static readonly IEnumerable<string> Keys = new List<string> { "MyKey1", "MyKey2" };

        public ValuesController(IConfiguration configuration)
        {
            _configs = Keys.ToDictionary(k => k, k => configuration[k] ?? string.Empty);
        }

        [HttpGet("")]
        public IEnumerable<string> Get()
        {
            return _configs.Values;
        }

        [HttpGet("{key}")]
        public string GetValueByKey(string key)
        {
            _configs.TryGetValue(key, out var value);
            return value;
        }
    }
}
