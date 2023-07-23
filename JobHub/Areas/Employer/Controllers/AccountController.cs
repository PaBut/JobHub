using JobHub.Areas.Employer.DTO;
using JobHub.Contracts;
using JobHub.Controllers;
using JobHub.DataAccess;
using JobHub.Enums;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model = JobHub.Models;

namespace JobHub.Areas.Employer.Controllers
{
    //TODO: Make a control for uploaded photo extention
    [Area("Employer")]
    [Route("[area]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Model.Employer> _userManager;
        private readonly SignInManager<Model.Employer> _signInManager;
        private readonly IFileDbUploader _fileDbUploader;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<Model.Employer> userManager,
            SignInManager<Model.Employer> signInManager,
            IFileDbUploader fileDbUploader,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileDbUploader = fileDbUploader;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "AnonymousOnly")]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnonymousOnly")]
        public async Task<IActionResult> Register(RegisterModelView registerMV)
        {
            if (ModelState.IsValid)
            {
                Guid? id = await _fileDbUploader.UploadFileAsync(registerMV.Photo);

                Model.Employer user = new Model.Employer()
                {
                    UserName = registerMV.Email,
                    Email = registerMV.Email,
                    CompanyName = registerMV.CompanyName,
                    CID = registerMV.CID,
                    Description = registerMV.Description,
                    PhotoId = id,
                };
                var result = await _userManager.CreateAsync(user, registerMV.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesEnum.Employer.ToString());
                    await _signInManager.PasswordSignInAsync(user, registerMV.Password, true, false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
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
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            FileModel photo = _unitOfWork.FileModelRepo.GetFirstOrDefault(f => f.Id == user.PhotoId);

            UpdateAccountModelView model = new UpdateAccountModelView
            {
                CompanyName = user.CompanyName,
                Email = user.Email,
                Photo = photo,
                Description = user.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Index(UpdateAccountModelView model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.Description = model.Description;
                Guid? oldPhotoId = user.PhotoId;
                Guid? newPhotoId = null;
                if (model.UpdatedPhoto != null)
                {
                    newPhotoId = await _fileDbUploader.UploadFileAsync(model.UpdatedPhoto);

                    user.PhotoId = newPhotoId;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if(model.UpdatedPhoto != null || oldPhotoId != null)
                    {
                        FileModel photoToDelete = _unitOfWork.FileModelRepo.GetFirstOrDefault(p => p.Id == oldPhotoId);
                        _unitOfWork.FileModelRepo.Delete(photoToDelete);
                    }
                }
                else
                {
                    if(model.UpdatedPhoto != null)
                    {
                        FileModel photoToDelete = _unitOfWork.FileModelRepo.GetFirstOrDefault(p => p.Id == newPhotoId);
                        _unitOfWork.FileModelRepo.Delete(photoToDelete);
                    }
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

            return View(model);
        }
    }
}
