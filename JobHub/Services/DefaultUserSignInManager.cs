using JobHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobHub.Services
{
    public class DefaultUserSignInManager : SignInManager<IdentityUser>
    {
        public DefaultUserSignInManager(UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<IdentityUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<IdentityUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, 
              optionsAccessor, logger, schemes, confirmation)
        {
        }

    }
}
