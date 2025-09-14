
using ClaimsMVC.Data.Models; 
using ClaimsMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// --- SERVICES CONFIGURATION ---

// 1. Add the Database Context
builder.Services.AddDbContext<ClaimsContextDB>(options =>
    options.UseSqlServer(connectionString));

// 2. Add the complete Identity System
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ClaimsContextDB>()
    .AddDefaultTokenProviders();

// 3. Configure Cookie Authentication (Standard for MVC)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // If a user is not logged in and tries to access a protected page,
        // they will be redirected to this URL.
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 4. Add the services for MVC Controllers and Views
builder.Services.AddControllersWithViews();

// 5. Add your custom application services
builder.Services.AddScoped<EmailService>();



// --- APPLICATION PIPELINE ---

var app = builder.Build();

// Seed the "Employee" and "CPD" roles on startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Employee", "CPD" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
// For CSS and JavaScript files

app.UseRouting();
app.UseStaticFiles();
// These must be in this order
app.UseAuthentication();
app.UseAuthorization();

// This sets up the default URL pattern for MVC (e.g., /Controller/Action/Id)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

