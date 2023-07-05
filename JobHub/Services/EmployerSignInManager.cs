using JobHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobHub.Services
{
    public class EmployerSignInManager : SignInManager<Employer>
    {
        public EmployerSignInManager(UserManager<Employer> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<Employer> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<Employer>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<Employer> confirmation)
        : base(userManager, contextAccessor, claimsFactory,
              optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
