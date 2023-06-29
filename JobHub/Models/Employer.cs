using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.Models
{
    public class Employer : IdentityUser
    {
        [Required]
        [StringLength(20)]
        public string? CompanyName { get; set; }
        
        public Guid? PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public FileModel? Photo { get; set; }

        [Required]
        [StringLength(25)]
        public string? CID { get; set; }

        public string? Description { get; set; }
    }
}
