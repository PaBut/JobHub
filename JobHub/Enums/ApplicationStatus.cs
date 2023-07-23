using System.ComponentModel.DataAnnotations;

namespace JobHub.Enums
{
    public enum ApplicationStatus
    {
        Sent,
        [Display(Name = "Next Stage")]
        NextStage,
        Selected,
        Declined
    }
}
