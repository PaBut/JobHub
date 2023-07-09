using JobHub.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.Models
{
    public class JobApplication
    {
        [Key]
        public Guid? Id { get; set; }

        public Guid? JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        public Job? Job { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Sent; 

        public string? ApplicantId { get; set; }
        [ForeignKey(nameof(ApplicantId))]
        public Applicant? Applicant { get; set; }

        public string? EmployerId { get; set; }
        [ForeignKey(nameof(EmployerId))]
        public Applicant? Employer { get; set; }

        public Guid? CVId { get; set; }
        [ForeignKey(nameof(CVId))]
        public FileModel? CV { get; set; }

        public string? AdditionalMessage { get; set; }

        public string? CompanyReply { get; set; }
    }
}
