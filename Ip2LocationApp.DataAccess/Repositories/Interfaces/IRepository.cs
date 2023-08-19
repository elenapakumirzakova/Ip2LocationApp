namespace Ip2LocationApp.DataAccess.Repositories.Interfaces;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(object id);
    Task InsertAsync(T obj);
    Task DeleteAsync(object id);
    Task SaveAsync();
    void Update(T obj);
}
