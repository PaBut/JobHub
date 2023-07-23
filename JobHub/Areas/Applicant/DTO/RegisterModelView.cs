using JobHub.Controllers;
using JobHub.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Areas.Applicant.DTO
{
    public class RegisterModelView
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Email must be in proper format")]
        [Remote(nameof(AccountController.IsEmailInUse), "Account", ErrorMessage = "Email is already in use")]
        [Required]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [RegularExpression("^[a-zA-Z. -]*$", ErrorMessage = "Name must be in proper format")]
        [StringLength(40)]
        public string? FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Required]
        [Display(Name = "Your latest education:")]
        public EducationType? EducationType { get; set; }

        public string? About { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password are not the same")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
