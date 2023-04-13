
namespace ObaGoupDataAccess.Repository.IRepository;
public interface IUnitOfWork
{
    IDocumentRepository document{ get; }
    IApplicationUserRepository ApplicationUser{get;}

    ICalendarRepository eventViewModel { get; }
    void Save();
}