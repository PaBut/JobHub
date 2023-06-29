using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Models
{
    public class DefaultUser : IdentityUser
    {
        [Required]
        [StringLength(40)]
        public string? FullName { get; set; }

    }
}
