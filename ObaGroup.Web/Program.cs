using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ObaGoupDataAccess;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.DataAccess.DbInitializer;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroup.Utility;
using ObaGroupUtility;

var builder = WebApplication.CreateBuilder(args);
var kvUri = builder.Configuration.GetSection("keyVaultUrl").Value;

IKeyVaultManager _keyVaultManager = new KeyVaultManager(new SecretClient(new Uri("https://obagroupkey.vault.azure.net/"),
    new DefaultAzureCredential()));

// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(_keyVaultManager.GetDbConnectionString()));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbIntializer, DbInitalizer>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IKeyVaultManager, KeyVaultManager>();
builder.Services.AddScoped<Icryption, Cryption>();
builder.Services.AddScoped<IGoogleTokensUtility, GoogleTokensUtility>();
builder.Services.AddScoped<IBlobUploader, BlobUploader>();
builder.Services.AddScoped<IOauth, OAuth>();
builder.Services.AddScoped<IOAuthTokenProperties, OAuthTokenProperties>();
builder.Services.AddSingleton(new SecretClient(new Uri(kvUri), new DefaultAzureCredential()));


builder.Services.AddControllersWithViews();


var googleSignInClientId = _keyVaultManager.GetGoogleSignInClientId();
var googleSignInClientSecret = _keyVaultManager.GetGoogleSignInClientSecret();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleSignInClientId;
    googleOptions.ClientSecret = googleSignInClientSecret;
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
builder.Services.AddDistributedMemoryCache();
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

app.MapControllerRoute(
    "default",
    "{area=Admin}/{controller=Account}/{action=Login}");

app.MapRazorPages();
app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
        dbInitializer.Initialize();
    }
}