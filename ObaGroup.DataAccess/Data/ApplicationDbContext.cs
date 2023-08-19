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
}


