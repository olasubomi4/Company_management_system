using System.Collections.Immutable;
using BulkyBook.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

using ObaGroup.Utility;
using Microsoft.Extensions.DependencyInjection;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));



builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbIntializer, DbInitalizer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();

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