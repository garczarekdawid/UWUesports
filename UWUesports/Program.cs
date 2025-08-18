using Microsoft.AspNetCore.Identity;
using UWUesports.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using UWUesports.Web.Services.Interfaces;
using UWUesports.Web.Models.Domain;
using UWUesports.Web.Services;
using UWUesports.Web.Repositories;
using UWUesports.Web.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<UWUesportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("uwuesports_db")));


// Dodaj Identity z w�asnym userem i rol� int:
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    // Konfiguracja opcji np. has�o, login itp.
    options.User.RequireUniqueEmail = true; // wymuszamy unikalny email
    options.SignIn.RequireConfirmedEmail = false; // mo�esz wymaga� potwierdzenia emaila
})
.AddEntityFrameworkStores<UWUesportDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, DummyEmailSender>();

builder.Services.AddScoped<IOrganizationRoleRepository, OrganizationRoleRepository>();
builder.Services.AddScoped<IOrganizationRoleService, OrganizationRoleService>();


builder.Services.AddScoped<IUserRoleAssignmentRepository, UserRoleAssignmentRepository>();
builder.Services.AddScoped<IUserRoleAssignmentService, UserRoleAssignmentService>();

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();

// Rejestracja serwisu użytkowników
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Dodaj uwierzytelnianie i autoryzacj�
app.UseAuthentication();  // <-- TU WA�NE: przed UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // <-- dodaj

//DataSeeder.SeedDatabase(app);

app.Run();
