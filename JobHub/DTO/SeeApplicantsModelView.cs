using JobHub.Models;
using System.ComponentModel.DataAnnotations;

namespace JobHub.DTO
{
    public class SeeApplicantsModelView
    {
        public IEnumerable<JobApplication>? ApplicationsList { get; set; }
        public Guid JobId { get; set; }
        public Guid ApplicationId { get; set; }
        
        [Display(Name = "Going to the next stage?:")]
        [Required]
        public bool IsApproved { get; set; }

        [Display(Name = "Your reply:")]
        [Required]
        public string Reply { get; set; }
    }
}
