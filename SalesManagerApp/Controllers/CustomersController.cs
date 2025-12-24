using Microsoft.AspNetCore.Mvc;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpPost("register-customer")]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpPut("update-customer/{id}")]
        public IActionResult Put()
        {
            return Ok();
        }

        [HttpDelete("delete-customer/{id}")]
        public IActionResult Delete()
        {
            return Ok();
        }

        [HttpGet("list-customers")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("get-customer/{id}")]
        public IActionResult GetById()
        {
            return Ok();
        }
    }
}
