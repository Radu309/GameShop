using Microsoft.EntityFrameworkCore;
using GameShop.Data;
using GameShop.Services;

var builder = WebApplication.CreateBuilder(args);
// connexion to postgresql

builder.Services.AddDbContext<GameShopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameShopContext")));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameShopContext")));

// builder.Services.AddDefaultIdentity<IdentityUser>(options => 
//      options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IdentityContext>();

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

app.MapRazorPages();
app.MapControllers();

app.Run();