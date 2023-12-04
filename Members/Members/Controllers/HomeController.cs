using Microsoft.AspNetCore.Mvc;

namespace Orders.Controllers
{

    [Route("/")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            string htmlContent = "<html><body><a href='swagger/index.html'>Swagger</a></body></html>";

            return Content(htmlContent, "text/html");
        }
    }

}

