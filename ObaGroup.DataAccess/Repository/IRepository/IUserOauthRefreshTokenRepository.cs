using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface IUserOauthRefreshTokenRepository: IRepository<UserOauthRefreshToken>
{
    void Upsert(UserOauthRefreshToken obj);
}
