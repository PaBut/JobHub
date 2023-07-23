using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model = JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using JobHub.Areas.Applicant.DTO;
using JobHub.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobHub.DataAccess;
using JobHub.Contracts;

namespace JobHub.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    [Route("[area]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Model.Applicant> _userManager;
        private readonly SignInManager<Model.Applicant> _signInManager;
        private readonly IEnumConverter _enumConverter;

        public AccountController(UserManager<Model.Applicant> userManager,
            SignInManager<Model.Applicant> signInManager,
            IEnumConverter enumConverter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _enumConverter = enumConverter;
        }

        [HttpGet]
        [Authorize(Policy = "AnonymousOnly")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy ="AnonymousOnly")]
        public async Task<IActionResult> Register(RegisterModelView registerMV)
        {
            if (ModelState.IsValid)
            {
                Model.Applicant user = new Model.Applicant() {
                    UserName = registerMV.Email,
                    Email = registerMV.Email,
                    FullName = registerMV.FullName,
                    Birthday = registerMV.Birthday,
                    About = registerMV.About,
                    EducationType = registerMV.EducationType
                };
                var result = await _userManager.CreateAsync(user, registerMV.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesEnum.Applicant.ToString());
                    await _signInManager.PasswordSignInAsync(user, registerMV.Password, true, false);
                    return RedirectToAction(nameof(JobHub.Controllers.HomeController.Index), "Home");
                }
                else
                {
                    IEnumerable<string> errors = result.Errors.Select(e => e.Description);
                    ViewBag.Errors = errors;
                    return View(registerMV);
                }
                
            }

            IEnumerable<string> errorList = ModelState.SelectMany(e => e.Value.Errors).
                Select(e => e.ErrorMessage);
            ViewBag.Errors = errorList;
            return View(registerMV);
        }

        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            UpdateAccountModelView model = new UpdateAccountModelView
            {
                FullName = user.FullName,
                Email = user.Email,
                Birthday = user.Birthday,
                About = user.About,
                Education = user.EducationType,
            };

            //SelectList educationTypes = new SelectList(Enum.GetNames(typeof(EducationType)));

            //educationTypes.FirstOrDefault(et => et.Text == user.EducationType.ToString()).Selected = true;

            ViewBag.EducationTypes = _enumConverter.GetSelectListFromEnum<EducationType>(user.EducationType.ToString());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Index(UpdateAccountModelView model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                user.Email = model.Email;
                user.EducationType = model.Education;
                user.About = model.About;

                
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    IEnumerable<string> errors = result.Errors.Select(e => e.Description);
                    ViewBag.Errors = errors;
                }
            }
            else
            {
                model.Email = user.Email;
                IEnumerable<string> errors = ModelState.SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage);
                ViewBag.Errors = errors;
            }

            //SelectList educationTypes = new SelectList(Enum.GetNames(typeof(EducationType)));

            //educationTypes.FirstOrDefault(et => et.Text == model.Education.ToString()).Selected = true;

            //ViewBag.EducationTypes = educationTypes;

            ViewBag.EducationTypes = _enumConverter.GetSelectListFromEnum<EducationType>(model.Education.ToString());

            return View(model);
        }
    }
}
