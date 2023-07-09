using JobHub.DataAccess;
using JobHub.Enums;
using JobHub.Filters;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobHub.Controllers
{
    [Route("/[controller]/[action]")]
    [Authorize(Roles = "Employer")]
    public class CompanyJobController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyJobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string id = GetUserId();
            IEnumerable<Job> jobList = _unitOfWork.JobRepo.GetAll().Where(j => j.EmployerId == id );
            return View(jobList);
        }

        //[Route("add")]
        [HttpGet]
        [TypeFilter(typeof(AddUpdateActionFilter))]
        [Route("/[controller]/[action]/{id?}")]
        public async Task<IActionResult> AddUpdateJob(Guid? id = null)
        {
            Job job = new Job();
            ViewBag.Method = "Add";
            if(id != null)
            {
                job = _unitOfWork.JobRepo.GetFirstOrDefault(j => j.Id == id);
                if(job == null)
                {
                    return NotFound();
                }
                ViewBag.Method = "Update";
            }
            return View(job);
        }

        [HttpPost]
        [TypeFilter(typeof(AddUpdateActionFilter))]
        [Route("/[controller]/[action]/{id?}")]
        public async Task<IActionResult> AddUpdateJob([FromForm]Job job, [FromRoute]Guid? id = null)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = ModelState.SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage);
                ViewBag.Errors = errors;
                if(id == null)
                {
                    ViewBag.Method = "Add";
                }
                else
                {
                    ViewBag.Method = "Update";
                }
                return View(job);
            }
            if (id == null)
            {
                job.EmployerId = GetUserId();
                _unitOfWork.JobRepo.Add(job);
            }
            else
            {
                _unitOfWork.JobRepo.Update(job);
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Route("/[controller]/[action]/{id?}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Job job = _unitOfWork.JobRepo.GetFirstOrDefault(j => j.Id == id);
            if(job == null)
            {
                return NotFound();
            }
            _unitOfWork.JobRepo.Delete(job);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            var claims = User.FindFirst(ClaimTypes.NameIdentifier);
            return claims.Value;
        }
    }
}
