namespace Ip2LocationApp.DataAccess.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(T obj)
    {
        await _dbSet.AddAsync(obj);
    }

    public void Update(T obj)
    {
        _dbSet.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public async Task DeleteAsync(object id)
    {
        T existing = await _dbSet.FindAsync(id);
        _dbSet.Remove(existing);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}