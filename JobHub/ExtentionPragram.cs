using JobHub.Contracts;
using JobHub.Data;
using JobHub.DataAccess;
using JobHub.Models;
using JobHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobHub
{
    public static class ExtentionPragram
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentityCore<DefaultUser>().
                AddRoles<IdentityRole>().
                AddDefaultTokenProviders().
                AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<Applicant>().
                AddRoles<IdentityRole>().
                AddDefaultTokenProviders().
                AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<Employer>().
                AddRoles<IdentityRole>().
                AddDefaultTokenProviders().
                AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Cookie.Name = IdentityConstants.ApplicationScheme;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            }).AddCookie(IdentityConstants.TwoFactorUserIdScheme, options =>
            {
                options.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            }).AddCookie(IdentityConstants.ExternalScheme, options =>
            {
                options.Cookie.Name = IdentityConstants.ExternalScheme;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AnonymousOnly", policy =>
                {
                    policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated);
                });

            });
            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileDbUploader, FileDbUploader>();
            services.AddScoped<SignInManager<DefaultUser>, DefaultUserSignInManager>();
            services.AddScoped<SignInManager<Applicant>, ApplicantSignInManager>();
            services.AddScoped<SignInManager<Employer>, EmployerSignInManager>();
        }
    }
}
