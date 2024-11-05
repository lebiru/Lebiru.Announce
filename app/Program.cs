using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lebiru.Announce.Services;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "yourdomain.com",
        ValidAudience = "yourdomain.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
    };
});

builder.Services.AddSingleton<AnnouncementService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<BannerService>(); // Register BannerService
builder.Services.AddControllers();
builder.Services.AddRazorPages(); // Add Razor Pages

var app = builder.Build();

var adminService = app.Services.GetRequiredService<AdminService>();

// First-time setup: Check if admin credentials exist
if (!adminService.AdminExists())
{
    Console.WriteLine("No admin account found. Setting up a new admin account.");
    Console.Write("Enter admin username: ");
    var username = Console.ReadLine();

    Console.Write("Enter admin password: ");
    var password = Console.ReadLine();

    adminService.CreateAdminAccount(username, password);
    Console.WriteLine("Admin account created successfully.");
}

app.UseStaticFiles(); // For CSS, JS, etc.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages(); // Map Razor Pages
app.Run();
