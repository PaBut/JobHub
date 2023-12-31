﻿using JobHub.Contracts;
using JobHub.Data;
using JobHub.DataAccess;
using JobHub.Models;
using JobHub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JobHub
{
    public static class ProgramExtentions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentityCore<IdentityUser>().
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options => options.Cookie.IsEssential = true);
            services.AddSession(options => options.Cookie.IsEssential = true);

            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileDbUploader, FileDbUploader>();
            services.AddScoped<SignInManager<IdentityUser>, DefaultUserSignInManager>();
            services.AddScoped<SignInManager<Applicant>, ApplicantSignInManager>();
            services.AddScoped<SignInManager<Employer>, EmployerSignInManager>();
            services.AddScoped<IEnumConverter, EnumConverter>();
        }

        public static string LimitString(this string limitedString, int charCount, string limitingString)
        {

            string result = new string(limitedString.Take(charCount).ToArray());
            if(limitedString.Length > charCount)
            {
                result += limitingString;
            }
            return result;
        }

        public static string GetDisplayValueName<T>(this T enumElement) where T : Enum
        {
            DisplayAttribute attribute = enumElement.GetType().GetMember(enumElement.ToString())
                .First().GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()
                as DisplayAttribute;

            return attribute == null ? enumElement.ToString() : attribute.Name;
        }
    }
}
