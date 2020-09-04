using Microsoft.AspNetCore.Mvc;

namespace UserManagerAPI.Controllers
{
    [Route( "/" )]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok( new { message = "Hello world! Test task by JK. Server is up and running ;)" } );
        }
    }
}
