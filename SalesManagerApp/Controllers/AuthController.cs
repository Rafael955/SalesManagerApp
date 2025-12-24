using Microsoft.AspNetCore.Mvc;

namespace SalesManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
