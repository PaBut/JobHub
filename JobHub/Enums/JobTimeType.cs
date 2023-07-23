using System.ComponentModel.DataAnnotations;

namespace JobHub.Enums
{
    public enum JobTimeType
    {
        [Display(Name = "Part-time")]
        Parttime, 
        [Display(Name = "Full-time")]
        Fulltime,
        Brigada
    }
}
