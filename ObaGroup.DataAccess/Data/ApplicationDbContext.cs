using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ObaGroupModel;
using ObaGroupUtility;

namespace ObaGoupDataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<EventViewModel> EventViewModels { get; set; }
    
    public DbSet<Biography> Biographies { get; set; }
    public DbSet<UserOauthRefreshToken> UserOauthRefreshToken { get; set; }

    public class YourDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:obagroup.database.windows.net,1433;Initial Catalog=ObaGroup;Persist Security Info=False;User ID=ObaGroup;Password=DareStagingApp456;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}


