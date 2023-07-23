
using JobHub.Controllers;
using JobHub.Enums;
using JobHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Areas.Employer.DTO
{
    public class UpdateAccountModelView
    {
        public string? CompanyName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email must be in proper format")]
        [Remote(nameof(AccountController.IsEmailInUse), "Account", ErrorMessage = "Email is already in use")]
        [Required]
        public string? Email { get; set; }

        public FileModel? Photo { get; set; }

        public IFormFile? UpdatedPhoto { get; set; }

        public string? Description { get; set; }
    }
}
