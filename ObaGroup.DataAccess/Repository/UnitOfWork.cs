using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        document = new DocumentRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        eventViewModel = new CalendarRepository(_db);
        biography = new BiographyRepository(_db);
        UserOauthRefreshTokenRepository = new UserOauthRefreshTokenRepository(_db);

    }
    public IDocumentRepository document { get; }
    public IApplicationUserRepository ApplicationUser{get;}
    public ICalendarRepository eventViewModel{get;}
    public IBiographyRepository biography { get; }
    
    public  IUserOauthRefreshTokenRepository UserOauthRefreshTokenRepository { get; }


    
    public void Save()
    {
        _db.SaveChanges();
    }
}