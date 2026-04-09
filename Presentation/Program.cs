using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Microsoft.AspNetCore.Authentication.Google;
using DataAccess.Context;
using Microsoft.Build.Framework;
using Presentation.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Lockout.MaxFailedAccessAttempts = 3; //idea is to block the account after the 3 failed consecutive login attempt

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 5; //Paaaaa@d
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
}
).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TicketContext>().AddDefaultTokenProviders().AddDefaultUI();
builder.Services.AddControllersWithViews();

// Register Razor Pages services for Identity UI (fix)
builder.Services.AddRazorPages();

builder.Services.AddAuthentication()
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


// Register repository using DI so TicketContext and IConfiguration are resolved automatically
builder.Services.AddScoped<DataAccess.Repositories.EventsRepository>();

var app = builder.Build();

// Create a scope to resolve scoped services (example - commented out)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();

    RolesManagementHelper rolesManagementHelper =
        new RolesManagementHelper(roleManager, userManager);

    rolesManagementHelper.DefaultRolesSetup();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();