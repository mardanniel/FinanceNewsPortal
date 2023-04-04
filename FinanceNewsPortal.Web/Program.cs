using FinanceNewsPortal.Web.Data;
using FinanceNewsPortal.Web.Models;
using FinanceNewsPortal.Web.Repository;
using FinanceNewsPortal.Web.Repository.Contracts;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<FinanceNewsPortalDbContext>();

builder.Services.AddScoped<INewsArticlesRepository, NewsArticlesRepository>();
builder.Services.AddScoped<IRatesRepository, RatesAPIRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FinanceNewsPortalDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Error/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
