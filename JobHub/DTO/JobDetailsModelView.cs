using JobHub.Models;
using System.ComponentModel.DataAnnotations;

namespace JobHub.DTO
{
    public class JobDetailsModelView
    {
        public Job? JobDetails { get; set; }

        [Required]
        public IFormFile? CV { get; set; }

        public string? AdditionalMessage { get; set; } 
    }
}
