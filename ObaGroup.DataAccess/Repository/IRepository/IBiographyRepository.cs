using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface IBiographyRepository : IRepository<Biography>
{
    void Update(Biography obj);
}