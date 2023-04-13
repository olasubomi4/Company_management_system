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
    
    public class YourDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=OBAGROUP1;Persist Security Info=False;User ID=sa;Password=MyPass@word;MultipleActiveResultSets=False");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}


