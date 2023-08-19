using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ObaGoupDataAccess;
using ObaGroup.Utility;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.DataAccess.DbInitializer;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupUtility;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

string kvUri = builder.Configuration.GetSection("keyVaultUrl").Value;

IKeyVaultManager _keyVaultManager = new KeyVaultManager(new SecretClient(new Uri(kvUri),
    new ClientSecretCredential("26b1c6e9-36aa-4650-8b9f-56efa2b6171b", "f770007e-660d-4906-9385-79733207df5e",
        "y6H8Q~C9jS3PgBSc_H8kQ7RlygiWSgYNSOXpRbRK")));

// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_keyVaultManager.GetDbConnectionString()));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbIntializer, DbInitalizer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped <IKeyVaultManager,KeyVaultManager>();
builder.Services.AddScoped<Icryption, Cryption>();
builder.Services.AddScoped<IGoogleTokensUtility, GoogleTokensUtility>();
builder.Services.AddScoped <IBlobUploader, BlobUploader>();
builder.Services.AddScoped<IOauth, OAuth>();
builder.Services.AddScoped<IOAuthTokenProperties, OAuthTokenProperties>();
// builder.Services.AddSingleton(new SecretClient(new Uri(kvUri), new DefaultAzureCredential()));

builder.Services.AddSingleton(new SecretClient(new Uri(kvUri),
    new ClientSecretCredential("26b1c6e9-36aa-4650-8b9f-56efa2b6171b", "f770007e-660d-4906-9385-79733207df5e",
        "y6H8Q~C9jS3PgBSc_H8kQ7RlygiWSgYNSOXpRbRK")));


var googleSignInClientId = _keyVaultManager.GetGoogleSignInClientId();
var googleSignInClientSecret = _keyVaultManager.GetGoogleSignInClientSecret();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleSignInClientId;
    googleOptions.ClientSecret =googleSignInClientSecret;
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = Constants.Access_Denied_Endpoint;
    options.LoginPath = Constants.Login_Endpoint;
    options.LogoutPath = Constants.Logout_Endpoint;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


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

