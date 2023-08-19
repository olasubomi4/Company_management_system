using System.Linq.Expressions;

namespace ObaGoupDataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    void Add(T entity);
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
}