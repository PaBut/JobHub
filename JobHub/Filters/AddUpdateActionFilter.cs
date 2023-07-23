using JobHub.Contracts;
using JobHub.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobHub.Filters
{
    public class AddUpdateActionFilter : IAsyncActionFilter
    {

        private IEnumConverter _enumConverter;

        public AddUpdateActionFilter(IEnumConverter enumConverter)
        {
            _enumConverter = enumConverter;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            next();
            //SelectList jobTimeTypes = new SelectList(Enum.GetNames(typeof(JobTimeType)));
            //SelectList educationTypes = new SelectList(Enum.GetNames(typeof(EducationType)));
            (context.Controller as Controller).ViewBag.JobTimeTypes = _enumConverter.GetSelectListFromEnum<JobTimeType>();
            (context.Controller as Controller).ViewBag.EducationTypes = _enumConverter.GetSelectListFromEnum<EducationType>();
        }
    }
}
