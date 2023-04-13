
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ObaGoupDataAccess.Data;
using ObaGroupModel;
using ObaGroupUtility;

namespace BulkyBook.DataAccess.DbInitializer;

public class DbInitalizer : IDbIntializer
{
     private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

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
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
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
                    Position = "developer"
                   
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "olasubomiodekunle@gmail.com");

                _userManager.AddToRoleAsync(user, Constants.Role_Admin).GetAwaiter().GetResult();

            }
            return;
        }
    }
