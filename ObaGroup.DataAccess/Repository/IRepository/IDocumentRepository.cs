using ObaGroupModel;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface IDocumentRepository : IRepository<Document>
{
    void Update(Document obj);
}