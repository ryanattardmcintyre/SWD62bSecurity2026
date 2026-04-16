using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Microsoft.AspNetCore.Authentication.Google;
using DataAccess.Context;
using Microsoft.Build.Framework;
using Presentation.Helpers;
using Serilog;


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


//Logs => files / cloud
//        reason not to log in a database: 
//        1- performance: logging in a database can be slower than logging in a file, especially if the database is on a different server or if there are a lot of logs to write.     
//        2- complexity: if the database fails or unavailable or no more space is left, logs will start failing with every line
                       // we're using logs as part of error handling
        //3- costs: database space costs more than storage for logs space



var myLogConfiguration = new LoggerConfiguration()
    .MinimumLevel.Warning() //=> Warning + Errors + Critical
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog(myLogConfiguration, dispose: true);
});

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

//this will catch any unhandled exceptions and user is redirected to the error page with the error details
//ReExecute => Server transfer
//Redirect => Client transfer //in case you want to log where it happened 
app.UseStatusCodePagesWithReExecute("/Home/StatusError", "?code={0}");


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