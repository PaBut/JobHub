using JobHub.Controllers;
using JobHub.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobHub.Filters
{
    public class RoleBasedStartAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(RolesEnum.Employer.ToString()))
            {
                context.Result = new RedirectToActionResult(nameof(CompanyJobController.Index), "CompanyJob", null);
            }
        }
    }
}
