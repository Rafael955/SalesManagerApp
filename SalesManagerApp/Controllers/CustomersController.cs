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
        public IActionResult Put([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpDelete("delete-customer/{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpGet("list-customers")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("get-customer/{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
