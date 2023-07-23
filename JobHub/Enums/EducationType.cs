using System.ComponentModel.DataAnnotations;

namespace JobHub.Enums
{
    public enum EducationType
    {
        University, 
        [Display(Name = "Middle School")]
        MiddleSchool,
        [Display(Name = "No Education")]
        NoEducation
    }
}
