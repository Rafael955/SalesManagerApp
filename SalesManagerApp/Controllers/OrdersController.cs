using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost("create-order")]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpGet("list-orders")]
        public IActionResult Get([FromQuery] int pageNumber,[FromQuery] int pageSize)
        {
            return Ok();
        }

        [HttpGet("update-order-status/{id}")]
        public IActionResult UpdateOrderStatus(Guid id, UpdateOrderStatusRequestDto request)
        {
            return Ok();
        }
    }
}
