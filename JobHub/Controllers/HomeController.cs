using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
