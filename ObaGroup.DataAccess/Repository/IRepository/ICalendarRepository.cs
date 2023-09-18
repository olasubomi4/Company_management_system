using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface ICalendarRepository : IRepository<EventViewModel>
{
    void Update(EventViewModel obj);
}