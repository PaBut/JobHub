using JobHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobHub.Services
{
    public class ApplicantSignInManager : SignInManager<Applicant>
    {
        public ApplicantSignInManager(UserManager<Applicant> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<Applicant> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<Applicant>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<Applicant> confirmation)
        : base(userManager, contextAccessor, claimsFactory,
              optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
