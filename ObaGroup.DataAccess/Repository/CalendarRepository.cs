using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository;

public class CalendarRepository : Repository<EventViewModel>, ICalendarRepository
{
    private ApplicationDbContext _db;

    public CalendarRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    
    public void Update(EventViewModel obj)
    {
        _db.EventViewModels.Update(obj);
    }
}
