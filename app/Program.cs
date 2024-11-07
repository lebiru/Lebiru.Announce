using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lebiru.Announce.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lebiru.Announce API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
