using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesManagerApp.Domain.Dtos.Requests;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            return Ok();
        }

        [HttpPost("alter-order")]
        public IActionResult AlterOrder([FromBody] CreateOrderRequestDto request)
        {
            return Ok();
        }

        [HttpDelete("cancel-order/{id}")]
        public IActionResult CancelOrder(Guid? id)
        {
            return Ok();
        }

        [HttpGet("update-order-status/{id}")]
        public IActionResult UpdateOrderStatus([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequestDto request)
        {
            return Ok();
        }

        [HttpGet("list-orders")]
        public IActionResult ListOrders([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            return Ok();
        }
    }
}
