using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext _db;

    public ApplicationUserRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ApplicationUser obj)
    {
        var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == obj.Id);
        if (objFromDb != null)
        {
            objFromDb.FirstName = obj.FirstName;
            objFromDb.LastName = obj.LastName;
            objFromDb.Address = obj.Address;
            objFromDb.Position = obj.Position;
        }

        _db.ApplicationUsers.Update(objFromDb);
    }
}