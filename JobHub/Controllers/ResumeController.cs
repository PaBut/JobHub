using JobHub.DataAccess;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHub.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResumeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("resume/{id}")]
        [Authorize]
        public async Task<IActionResult> GetResume(Guid id)
        {
            FileModel cv = _unitOfWork.FileModelRepo.GetFirstOrDefault(f => f.Id == id);
            return File(cv.Data, cv.Extention);
        }
    }
}
