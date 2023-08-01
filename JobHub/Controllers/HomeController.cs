using JobHub.Contracts;
using JobHub.DataAccess;
using JobHub.DTO;
using JobHub.Enums;
using JobHub.Filters;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileDbUploader _fileDbUploader;
        private readonly IEnumConverter _enumConverter;
        private readonly int _elementsPerPage;

        public HomeController(IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IFileDbUploader fileDbUploader,
            IEnumConverter enumConverter)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _elementsPerPage = _configuration.GetValue<int>("ElementsPerPage");
            _fileDbUploader = fileDbUploader;
            _enumConverter = enumConverter;
        }

        [Route("/")]
        [HttpGet]
        [TypeFilter(typeof(RoleBasedStartAuthorizationFilter))]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        [HttpGet]
        [Route("/privacy")]
        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("/jobs")]
        public async Task<ActionResult> JobList(PagingJobModel model)
        {
            if (model.SortOptions == null)
            {
                model.SortOptions = SortOptions.ByNameDescending;
            }
            IEnumerable<Job> jobs = _unitOfWork.JobRepo.ExecuteCustomQuery(dbset =>
            {
                IQueryable<Job> result = dbset.AsQueryable();

                if (model.JobName != null)
                {
                    result = result.Where(j => j.Name.Contains(model.JobName));
                }

                if (model.Place != null)
                {
                    result = result.Where(j => j.City.Contains(model.Place) ||
                    j.Region.Contains(model.Place) ||
                    (j.Region + "," + j.City).Contains(model.Place));
                }

                if (model.MinimumWage != null)
                {
                    result = result.Where(j => j.Wage >= model.MinimumWage);
                }

                if (model.TimeType != null)
                {
                    var selectedTimeTypes = model.TimeType.Where(t => t.Value).Select(t => t.Key).ToList();
                    if (selectedTimeTypes.Any())
                    {
                        result = result.Where(j => selectedTimeTypes.Contains(j.TimeType.Value));
                    }
                }

                if (model.EducationRequired != null)
                {
                    var selectedEducation = model.EducationRequired.Where(e => e.Value).Select(e => e.Key).ToList();
                    if (selectedEducation.Any())
                    {
                        result = result.Where(j => selectedEducation.Contains(j.EducationRequired.Value));
                    }
                }

                result = model.SortOptions switch
                {
                    SortOptions.RecentlyPosted => result.OrderByDescending(j => j.PostDate),
                    SortOptions.HighestWage => result.OrderByDescending(j => j.Wage),
                    SortOptions.ByNameAscending => result.OrderBy(j => j.Name),
                    SortOptions.ByNameDescending => result.OrderByDescending(j => j.Name),
                    _ => result.OrderBy(j => j.Name)
                };
                return result.Include(nameof(Job.Employer)).Include($"{nameof(Job.Employer)}.{nameof(Job.Employer.Photo)}");
            });

            model.PageSize = _elementsPerPage;
            model.SelectPageElements(jobs);

            //SelectList sortOptions = new SelectList(Enum.GetNames(typeof(SortOptions)));
            //sortOptions.Where(o => o.Text == model.SortOptions.ToString()).First().Selected = true;
            ViewBag.SortOptions = _enumConverter.GetSelectListFromEnum<SortOptions>(model.SortOptions.ToString());

            return View(model);
        }

        [HttpGet]
        [Route("jobdetails/{id}")]
        public async Task<IActionResult> JobDetails(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Job job = _unitOfWork.JobRepo.GetFirstOrDefault(j => j.Id == id,
                $"{nameof(Job.Employer)},{nameof(Job.Employer)}.{nameof(Job.Employer.Photo)}");
            if (job == null)
            {
                return NotFound();
            }

            JobDetailsModelView jobDetailsMV = new JobDetailsModelView()
            {
                JobDetails = job
            };

            return View(jobDetailsMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("jobdetails/{id}")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> JobDetails(JobDetailsModelView model)
        {
            string extention = model.CV.FileName.Split('.').LastOrDefault();
            if (extention == "pdf" || extention == "docx" || extention == "doc" || extention == "txt")
            {
                Guid? fileId = await _fileDbUploader.UploadFileAsync(model.CV);
                JobApplication application = new JobApplication
                {
                    JobId = model.JobDetails.Id,
                    EmployerId = model.JobDetails.EmployerId,
                    CVId = fileId,
                    AdditionalMessage = model.AdditionalMessage,
                    ApplicantId = User.FindFirst(ClaimTypes.NameIdentifier).Value
                };
                _unitOfWork.JobApplicationRepo.Add(application);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(JobList), "Home");
            }

            ModelState.AddModelError(nameof(model.CV), "File must have .pdf, .docx, .doc or .txt extention");

            if (model.JobDetails.Id == null)
            {
                return BadRequest();
            }
            Job job = _unitOfWork.JobRepo.GetFirstOrDefault(j => j.Id == model.JobDetails.Id,
                $"{nameof(Job.Employer)},{nameof(Job.Employer)}.{nameof(Job.Employer.Photo)}");
            if (job == null)
            {
                return NotFound();
            }
            model.JobDetails = job;

            return View(model);
        }

        [HttpGet]
        [Route("applications")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> JobApplications()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<JobApplication> applications = _unitOfWork.JobApplicationRepo.GetRange(ja => ja.ApplicantId == id,
                $"{nameof(JobApplication.Employer)},{nameof(JobApplication.Job)},{nameof(JobApplication.CV)}," +
                $"{nameof(JobApplication.Employer)}.{nameof(JobApplication.Employer.Photo)}");

            return View(applications);
        }
    }
}
