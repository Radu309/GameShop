using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Models;
using GameShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Înregistrarea serviciului pentru trimiterea emailurilor
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()  
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
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("Admin", policy => {
        policy.RequireClaim("Admin");
    });
    opts.AddPolicy("Customer", policy => {
        policy.RequireClaim("Customer");
    });
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expirare sesiune
});


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
    pattern: "{controller=Games}/{action=Index}/{id?}"
    );
app.MapControllers();
app.MapRazorPages();

// Adaugă apelul pentru seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedRolesAndUsers(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Eroare în timpul seeding-ului: {ex.Message}");
    }
}

app.Run();