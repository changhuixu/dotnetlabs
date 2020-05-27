using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderApi.RabbitMQ;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private static readonly List<Order> Orders = new List<Order>();
        private readonly RabbitMqClient _rabbitMqClient;

        public OrdersController(ILogger<OrdersController> logger, RabbitMqClient rabbitMqClient)
        {
            _logger = logger;
            _rabbitMqClient = rabbitMqClient;
        }

        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return Orders;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var order = Orders.SingleOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post(NewOrderRequest request)
        {
            var newOrder = new Order
            {
                Id = Orders.Select(x => x.Id).DefaultIfEmpty().Max() + 1,
                Email = request.Email
            };
            Orders.Add(newOrder);
            var payload = JsonSerializer.Serialize(newOrder);
            _logger.LogInformation($"New order created: {payload}");

            _rabbitMqClient.Publish("ordering", "order.created", payload);

            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class NewOrderRequest
    {
        public string Email { get; set; }
    }
}