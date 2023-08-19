using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser applicationUser);
}