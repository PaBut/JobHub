//using JobHub.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace JobHub.DTO
//{
//    public class RegisterEmployerModelView
//    {
//        [DataType(DataType.EmailAddress, ErrorMessage = "Email must be in proper format")]
//        [Remote(nameof(AccountController.IsEmailInUse), "Account", ErrorMessage = "Email is already in use")]
//        [Required]
//        public string? Email { get; set; }

//        [Required]
//        [StringLength(20)]
//        [Display(Name = "Company Name")]
//        public string? CompanyName { get; set; }

//        public IFormFile? Photo { get; set; }

//        public string? Description { get; set; }

//        [Required]
//        [StringLength(25)]
//        public string? CID { get; set; }

//        [DataType(DataType.Password)]
//        [Required]
//        public string? Password { get; set; }

//        [DataType(DataType.Password)]
//        [Required]
//        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password are not the same")]
//        [Display(Name = "Confirm Password")]
//        public string? ConfirmPassword { get; set; }
//    }
//}
