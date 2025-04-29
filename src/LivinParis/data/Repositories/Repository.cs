using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <inheritdoc/>
public class Repository<T> : IRepository<T>
    where T : class
{
    #region Fields

    protected readonly LivInParisContext _context;
    protected readonly DbSet<T> _dbSet;

    #endregion Fields

    #region Constructor

    public Repository(LivInParisContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    #endregion Constructor

    #region Methods

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<T>> ReadAsync(Func<T, bool>? predicate = null)
    {
        var data = await _dbSet.ToListAsync();
        return predicate is null ? data : data.Where(predicate);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    #endregion Methods
}
