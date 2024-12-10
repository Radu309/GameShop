using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;
using GameShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
// connexion to postgresql

// Înregistrarea serviciului pentru trimiterea emailurilor
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// builder.Services.AddIdentity<AppUser, IdentityRole>()
//     .AddEntityFrameworkStores<IdentityContext>()
//     .AddDefaultTokenProviders();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<GameShopContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<GameShopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameShopContextConnection")));

// builder.Services.AddDbContext<IdentityContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityContextConnection")));


// CORS policy support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<GameService>();
builder.Services.AddRazorPages(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Active CORS policy
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Games}/{action=Index}/{id?}");

app.MapControllers();
app.MapRazorPages();
app.MapControllers();

app.Run();