using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost("create-product")]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpPut("update-product/{id}")]
        public IActionResult Put()
        {
            return Ok();
        }

        [HttpDelete("delete-product/{id}")]
        public IActionResult Delete()
        {
            return Ok();
        }

        [HttpGet("list-products")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("get-product/{id}")]
        public IActionResult GetById()
        {
            return Ok();
        }
    }
}
