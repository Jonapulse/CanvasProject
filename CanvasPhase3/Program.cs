using CanvasPhase3.CanvasPhase3.Context;
using Microsoft.EntityFrameworkCore;
using LMS.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CanvasPhase3.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CanvasPhase3ContextConnection") ?? throw new InvalidOperationException("Connection string 'CanvasPhase3ContextConnection' not found.");;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["LMS:ConnectionString"]);
});

builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseNpgsql(builder.Configuration["LMS:IdentityConnectionString"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
        options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//adapted from https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await Roles.SeedRolesAsync(roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();