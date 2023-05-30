using System.Collections.Immutable;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

using ObaGroup.Utility;
using Microsoft.Extensions.DependencyInjection;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.DataAccess.DbInitializer;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupUtility;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));


/*builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("GoogleAuthSettings")
        .GetValue<string>("ClientId");
    googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings")
        .GetValue<string>("ClientSecret");
});
*/

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddCors();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbIntializer, DbInitalizer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession(options =>
{
    // Set a timeout for the session
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.AccessDeniedPath = "/MyHttpStatuses/AccessDenied";
//});
/*builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "X-CSRF-TOKEN";
    options.HeaderName = "X-CSRF-TOKEN";
});
*/
/*builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit =  500 * 1024 * 1024; // 50 MB
});
*/

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

//app.UseStatusCodePagesWithReExecute(Constants.Access_Denied_Endpoint);
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
        dbInitializer.Initialize();
    }
}