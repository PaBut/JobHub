﻿using JobHub.Areas.Employer.Controllers;
using JobHub.Contracts;
using JobHub.DTO;
using JobHub.Enums;
using JobHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobHub.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IFileDbUploader _fileDbUploader;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IFileDbUploader fileDbUploader)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileDbUploader = fileDbUploader;
        }

        [HttpGet]
        [Authorize(Policy = "AnonymousOnly")]
        public async Task<IActionResult> LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnonymousOnly")]
        public async Task<IActionResult> LogIn(LogInModelView model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = ModelState.SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage);
                ViewBag.Errors = errors;
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (result.Succeeded)
            {
                if (User.IsInRole(RolesEnum.Employer.ToString()))
                {
                    return RedirectToAction(nameof(CompanyJobController.Index), "CompanyJob", new {area = "Employer"});
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                IEnumerable<string> errors = new List<string> { "Email or Password are not correct"};
                ViewBag.Errors = errors;
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> IsEmailInUse(string? email)
        {
            if(User.Identity.IsAuthenticated && email == User.FindFirst(ClaimTypes.Email).Value)
            {
                return Json(true);
            }
            if (_userManager.Users.Any(u => u.Email == email))
                return Json(false);
            return Json(true);
        }
    }
}
