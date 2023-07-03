using JobHub.Data;
using JobHub.DataAccess;
using JobHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentityCore<DefaultUser>().
    AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Applicant>().
    AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Employer>().
    AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

app.UseStaticFiles();
app.UseHsts();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.MapControllers();

app.Run();
