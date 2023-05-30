using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ObaGroupModel;

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
            optionsBuilder.UseSqlServer( "Data Source=.\\sqlexpress;Initial Catalog=docmanager;Integrated Security=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}


