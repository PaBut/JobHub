using JobHub.DataAccess;
using JobHub.DTO;
using JobHub.Enums;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly int _elementsPerPage;

        public HomeController(IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _elementsPerPage = _configuration.GetValue<int>("ElementsPerPage");
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("/jobs")]
        public async Task<ActionResult> JobList(PagingJobModel model)
        {
            if(model.SortOptions == null)
            {
                model.SortOptions = SortOptions.ByNameDescending;
            }
            IEnumerable<Job> jobs = _unitOfWork.JobRepo.ExecuteCustomQuery(dbset =>
            {
                IQueryable<Job> result = dbset.AsQueryable();

                if(model.JobName != null)
                {
                    result = result.Where(j => j.Name.Contains(model.JobName));
                }

                if (model.Place != null)
                {
                    result = result.Where(j => j.City.Contains(model.Place) ||
                    j.Region.Contains(model.Place) ||
                    String.Join(',', j.Region, j.City).Contains(model.Place));
                }

                if(model.MinimumWage != null)
                {
                    result = result.Where(j => j.Wage >= model.MinimumWage);
                }

                if(model.TimeType != null)
                {
                    var selectedTimeTypes = model.TimeType.Where(t => t.Value).Select(t => t.Key).ToList();
                    if (selectedTimeTypes.Any())
                    {
                        result = result.Where(j => selectedTimeTypes.Contains(j.TimeType.Value));
                    }
                }

                if(model.EducationRequired != null)
                {
                    var selectedEducation = model.EducationRequired.Where(e => e.Value).Select(e => e.Key).ToList();
                    if (selectedEducation.Any())
                    {
                        result = result.Where(j => selectedEducation.Contains(j.EducationRequired.Value));
                    }
                }

                result = model.SortOptions switch { 
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

            SelectList sortOptions = new SelectList(Enum.GetNames(typeof(SortOptions)));
            sortOptions.Where(o => o.Text == model.SortOptions.ToString()).First().Selected = true;
            ViewBag.SortOptions = sortOptions;


            //List<SelectListItem> timeTypes = new List<SelectListItem>();
            //foreach(var type in model.TimeType)
            //{
            //    timeTypes.Add(new SelectListItem(Enum.GetName(type.Item1), type.Item2.ToString(), type.Item2));
            //}
            //ViewBag.TimeTypes = timeTypes;

            //List<SelectListItem> educationTypes = new List<SelectListItem>();
            //foreach (var type in model.EducationRequired)
            //{
            //    timeTypes.Add(new SelectListItem(Enum.GetName(type.Item1), true.ToString(), type.Item2));
            //}
            //ViewBag.EducationTypes = educationTypes;

            return View(model);
        }

        [HttpGet]
        [Route("jobdetails/{id}")]
        public async Task<IActionResult> JobDetails(Guid? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            Job job = _unitOfWork.JobRepo.GetFirstOrDefault(j => j.Id == id,
                $"{nameof(Job.Employer)},{nameof(Job.Employer)}.{nameof(Job.Employer.Photo)}");
            if(job == null)
            {
                return NotFound();
            }

            JobDetailsModelView jobDetailsMV = new JobDetailsModelView()
            {
                JobDetails = job
            };

            return View(jobDetailsMV);
        }
    }
}
