using System.ComponentModel.DataAnnotations;

namespace JobHub.Models
{
    public class FileModel
    {
        [Key]
        public Guid? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public byte[]? Data { get; set; }

        [Required]
        [StringLength(10)]
        public string? Extention { get; set; }
    }
}
