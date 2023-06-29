using JobHub.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobHub.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyJobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[Route("add")]
        [HttpGet]
        public async Task<IActionResult> AddJob()
        {
            SelectList jobTimeTypes = new SelectList(Enum.GetNames(typeof(JobTimeType)));
            SelectList educationTypes = new SelectList(Enum.GetNames(typeof(EducationType)));
            ViewBag.JobTimeTypes = jobTimeTypes;
            ViewBag.EducationTypes = educationTypes;
            return View();
        }
    }
}
