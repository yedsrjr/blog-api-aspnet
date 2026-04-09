using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok();
        }

    }
}
