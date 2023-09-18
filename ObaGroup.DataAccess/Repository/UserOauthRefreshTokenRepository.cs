using ObaGoupDataAccess.Data;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public class UserOauthRefreshTokenRepository : Repository<UserOauthRefreshToken>, IUserOauthRefreshTokenRepository
{
    private readonly ApplicationDbContext _db;

    public UserOauthRefreshTokenRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(UserOauthRefreshToken obj)
    {
        _db.UserOauthRefreshToken.Update(obj);
    }
}