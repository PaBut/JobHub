using JobHub.Enums;
using JobHub.Models;
using System.ComponentModel.DataAnnotations;

namespace JobHub.DTO
{
    //public class PagingJobModel : List<Job>
    //{
    //    [Required]
    //    public string? JobName { get; set; }
    //    public string? Place { get; set; }
    //    public SortOptions? SortOptions { get; set; }
    //    public int? MinimumWage { get; set; }
    //    public List<(JobTimeType, bool)> TimeType { get; set; } = new List<(JobTimeType, bool)> 
    //    { 
    //        (JobTimeType.Brigada, false),
    //        (JobTimeType.Fulltime, false),
    //        (JobTimeType.Parttime, false)
    //    };
    //    public List<(EducationType, bool)> EducationRequired { get; set; } = new List<(EducationType, bool)>
    //    {
    //        (EducationType.NoEducation, false),
    //        (EducationType.University, false),
    //        (EducationType.MiddleSchool, false)
    //    };

    //    public int PageSize { get; set; } = 0;
    //    public int PageIndex { get; set; } = 1;
    //    public int PageCount { get; set; } = 0;

    //    public bool HasPreviousPage => PageIndex > 1;
    //    public bool HasNextPage => PageIndex <= PageCount;

    //    public void SelectPageElements(IEnumerable<Job> jobs)
    //    {
    //        PageCount = Convert.ToInt32(Math.Ceiling(jobs.Count() / (double)PageSize));
    //        jobs = jobs.Skip((PageIndex - 1) * PageSize).Take(PageSize);
    //        base.AddRange(jobs);
    //    } 
    //}
    public class PagingJobModel
    {
        [Required]
        public string? JobName { get; set; }
        public List<Job> JobList { get; set; } = new List<Job>();
        public string? Place { get; set; }
        public SortOptions? SortOptions { get; set; }
        public int? MinimumWage { get; set; }
        public Dictionary<JobTimeType, bool> TimeType { get; set; } = new Dictionary<JobTimeType, bool>
        {
            { JobTimeType.Brigada, false },
            { JobTimeType.Fulltime, false },
            { JobTimeType.Parttime, false }
        };
        public Dictionary<EducationType, bool> EducationRequired { get; set; } = new Dictionary<EducationType, bool>
        {
            { EducationType.NoEducation, false },
            { EducationType.University, false },
            { EducationType.MiddleSchool, false }
        };

        public int PageSize { get; set; } = 0;
        public int PageIndex { get; set; } = 1;
        public int PageCount { get; set; } = 0;

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < PageCount;

        public void SelectPageElements(IEnumerable<Job> jobs)
        {
            PageCount = Convert.ToInt32(Math.Ceiling(jobs.Count() / (double)PageSize));
            if (PageIndex < 1)
            {
                PageIndex = 1;
            }
            if(PageIndex > PageCount)
            {
                PageIndex = PageCount;
            }
            jobs = jobs.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            JobList.AddRange(jobs);
        }
    }
}
