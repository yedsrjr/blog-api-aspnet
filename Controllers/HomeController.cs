using Blog.Attibutes;
using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [ApiKey]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        
        public IActionResult Get()
        {
            return Ok();
        }

    }
}
