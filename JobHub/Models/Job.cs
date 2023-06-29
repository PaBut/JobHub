using JobHub.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Models
{
    public class Job
    {
        [Key]
        public Guid? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public DateTime PostDate { get; } = DateTime.Now;
        [Required]
        public string? Description { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public int? Wage { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Region { get; set; }

        [Required]
        public JobTimeType? TimeType { get; set; }
        [Required]
        public EducationType? EducationRequired { get; set; }

    }
}
