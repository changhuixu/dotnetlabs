using System;
using Microsoft.AspNetCore.Mvc;

namespace ThrottledWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("isPrime")]
        public ActionResult<bool> Get(int number)
        {
            try
            {
                return Ok(IsPrime(number));
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(nameof(number), e.Message);
                return BadRequest(ModelState);
            }
        }

        // TEST CASES:
        // 1,4,6,8,9,10 --> false;
        // 2,3,5,7,11   --> true;
        // 11,13,17,19,23,29,31,41,43 -> true;
        // 1763=41*43   --> false;
        // 7400854980481283=86028221*86028223 -> false;
        private static bool IsPrime(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Only valid positive numbers are supported.");
            }
            if (number == 1)
            {
                return false;
            }
            if (number == 2 || number == 3 || number == 5)
            {
                return false;
            }
            if (number % 2 == 0 || number % 3 == 0 || number % 5 == 0)
            {
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));
            var i = 6;
            while (i <= boundary)
            {
                if (number % (i + 1) == 0 || number % (i + 5) == 0) return false;
                i += 6;
            }
            return true;
        }
    }
}
