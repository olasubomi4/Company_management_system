using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ObaGoupDataAccess.Data;
using ObaGroupModel;
using ObaGroupUtility;

namespace ObaGoupDataAccess.DataAccess.DbInitializer;

public class DbInitalizer : IDbIntializer
{
    private readonly ApplicationDbContext _db;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public DbInitalizer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
    }


    public void Initialize()
    {
        //migrations if they are not applied
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
        }
        catch (Exception ex)
        {
        }

        //create roles if they are not created
        if (!_roleManager.RoleExistsAsync(Constants.Role_Admin).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Constants.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Role_Staff)).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "olasubomiodekunle@gmail.com",
                Email = "olasubomiodekunle@gmail.com",
                FirstName = "subomi",
                LastName = "odekunle",
                PhoneNumber = "1112223333",
                Position = "developer",
                ImageUrl =
                    "data:image/png;base64,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"
            }, "Admin123*").GetAwaiter().GetResult();

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "olasubomiodekunle@gmail.com");

            _userManager.AddToRoleAsync(user, Constants.Role_Admin).GetAwaiter().GetResult();
        }
    }
}