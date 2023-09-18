using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess.Repository;

public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    private readonly ApplicationDbContext _db;

    public DocumentRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Document obj)
    {
        _db.Documents.Update(obj);
    }
}