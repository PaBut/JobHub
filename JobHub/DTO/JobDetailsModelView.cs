using JobHub.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace JobHub.DTO
{
    public class JobDetailsModelView
    {
        [ValidateNever]
        public Job? JobDetails { get; set; }

        [Required]
        [FileExtensions(Extensions = ".pdf,.docx,.txt")]
        public IFormFile? CV { get; set; }

        [Display(Name = "Additional Message")]
        public string? AdditionalMessage { get; set; } 
    }
}
