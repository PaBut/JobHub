using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobHub.Enums
{
    public enum SortOptions
    {
        [Display(Name = "By Highest Wage")]
        HighestWage,
        [Display(Name = "By Name Ascending")]
        ByNameAscending,
        [Display(Name = "By Name Descending")]
        ByNameDescending,
        [Display(Name = "Recently Posted")]
        RecentlyPosted
    }
}
