using JobHub.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.Models
{
    public class Applicant : IdentityUser
    {
        [Required]
        //[Display(Name = "Full Name")]
        //[RegularExpression("^[a-zA-Z.- ]*$", ErrorMessage = "Name must be in proper format")]
        [StringLength(40)]
        public string? FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [NotMapped]
        public int? Age { get => Birthday.Value.Year - DateTime.Now.Year; }

        public EducationType? EducationType { get; set; }

        public string? About { get; set; }

    }
}
