using JobHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace JobHub.Services
{
    public class DefaultUserSignInManager : SignInManager<DefaultUser>
    {
        public DefaultUserSignInManager(UserManager<DefaultUser> userManager,
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<DefaultUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<DefaultUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<DefaultUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, 
              optionsAccessor, logger, schemes, confirmation)
        {
        }

    }
}
