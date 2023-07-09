using JobHub;
using JobHub.Enums;
using Microsoft.AspNetCore.Identity;

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
        if (!await roleManager.RoleExistsAsync(Enum.GetName(RolesEnum.Admin)))
        {
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(RolesEnum.Admin)));
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(RolesEnum.Employer)));
            await roleManager.CreateAsync(new IdentityRole(Enum.GetName(RolesEnum.Applicant)));
        }
    }
}
