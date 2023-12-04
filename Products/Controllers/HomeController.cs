using Microsoft.AspNetCore.Mvc;

namespace Products.Controllers
{

    [Route("/")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, this is the root endpoint!");
        }
    }

}

