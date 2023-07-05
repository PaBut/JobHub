using JobHub;
using JobHub.Contracts;
using JobHub.Data;
using JobHub.DataAccess;
using JobHub.Models;
using JobHub.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.ConfigureServices(configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseHsts();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await AddRoles(app);

app.Run();


async Task AddRoles(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(SD.AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(SD.AdminRole));
            await roleManager.CreateAsync(new IdentityRole(SD.EmployerRole));
            await roleManager.CreateAsync(new IdentityRole(SD.ApplicantRole));
        }
    }
}
