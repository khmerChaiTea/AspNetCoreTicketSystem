using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem;
using AspNetCoreTicketSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// In ConfigureServices method or equivalent
builder.Services.AddScoped<ITicketSystemService, TicketSystemService>();

var app = builder.Build();

// Called the method p.92
InitializeDatabase(app);

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

app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// We no longer use IWebHost host; we use WebApplication app
// using Microsoft.Extensions.DependencyInjection;
static void InitializeDatabase(WebApplication app)
{
    // use app instead of host p.92
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Use try and catch sparingly
        try
        {
            SeedData.InitializeAsync(services).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error occurred while seeding database.");
        }
    }
}

public partial class Program { }

// This code configures an ASP.NET Core application,
// setting up essential services like Entity Framework Core,
// Identity, and custom services. It initializes the database
// with seed data during startup, configures the HTTP request
// pipeline for both development and production environments,
// and sets up routing and middleware necessary for the application to run