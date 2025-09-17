
namespace Restaurant.PL.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("~/Views/Error/NotFound.cshtml"); 
            }

            return View("~/Views/Error/Error.cshtml"); 
        }
        [Route("Error")]
        public IActionResult Error()
        {
            return View("~/Views/Error/Error.cshtml");
        }
    }
}
