
using JobHub.Controllers;
using JobHub.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Areas.Applicant.DTO
{
    public class UpdateAccountModelView
    {
        public string? FullName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email must be in proper format")]
        [Remote(nameof(AccountController.IsEmailInUse), "Account", ErrorMessage = "Email is already in use")]
        [Required]
        public string? Email { get; set; }

        public DateTime? Birthday { get; set; }
        
        [Required]
        public EducationType? Education { get; set; }    
        
        public string? About { get; set; }
    }
}
