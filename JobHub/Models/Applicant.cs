using JobHub.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.Models
{
    public class Applicant : IdentityUser
    {
        [Required]
        [StringLength(40)]
        public string? FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [NotMapped]
        public int? Age { get => DateTime.Now.Year - Birthday.Value.Year; }

        public EducationType? EducationType { get; set; }

        public string? About { get; set; }

    }
}
