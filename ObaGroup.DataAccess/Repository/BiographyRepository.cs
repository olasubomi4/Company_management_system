using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository;

public class BiographyRepository: Repository<Biography>, IBiographyRepository
{
    private ApplicationDbContext _db;

    public BiographyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    
    public void Update(Biography obj)
    {
        _db.Biographies.Update(obj);
    }
}
