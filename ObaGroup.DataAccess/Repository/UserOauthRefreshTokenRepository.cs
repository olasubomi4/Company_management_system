using ObaGoupDataAccess.Data;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public class UserOauthRefreshTokenRepository : Repository<UserOauthRefreshToken>, IUserOauthRefreshTokenRepository
{
    private ApplicationDbContext _db;

    public UserOauthRefreshTokenRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Upsert(UserOauthRefreshToken obj)
    {
        var refreshTokenObj = _db.UserOauthRefreshToken.FirstOrDefault(u => u.ApplicationUser.Email == obj.ApplicationUser.Email);

        if (refreshTokenObj== null)
        {
          /* var userIdFromDb = refreshTokenObj.Email;
            if (string.IsNullOrWhiteSpace(userIdFromDb))
            {
            */
                _db.UserOauthRefreshToken.Add(obj);
                _db.SaveChanges();
           // }
        }

        _db.UserOauthRefreshToken.Update(obj);
    }
}