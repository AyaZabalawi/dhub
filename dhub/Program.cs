using dhub.Data;
using dhub.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using dhub.Performance;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageSurveysPolicy", policy =>
    {
        policy.RequireRole("Owner");
    });
    options.AddPolicy("EditSurveyPolicy", policy =>
    {
        policy.RequireRole("Owner");
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await RoleSeeder.SeedRoles(roleManager);
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    ApplicationDbInitializer.Seed(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseMiddleware<PerformanceLoggingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Survey}/{action=Index}/{id?}");

app.Run();
