using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System.Linq.Expressions;

namespace Repository;
public class Repository<T> : IRepository<T> where T : class
{
    protected FUFlowerBouquetManagementContext _context;
    protected DbSet<T> dbSet;
    public Repository(
        FUFlowerBouquetManagementContext _context)
    {
        _context = _context;
        dbSet = _context.Set<T>();
    }
    public async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>> filter = null,
        int first = 0, int offset = 0,
        params string[] navigationProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (offset > 0)
        {
            query = query.Skip(offset);
        }
        if (first > 0)
        {
            query = query.Take(first);
        }

        query = ApplyNavigation(query, navigationProperties);
        return await query.ToListAsync();
    }

    private IQueryable<T> ApplyNavigation(IQueryable<T> query, params string[] navigationProperties)
    {
        foreach (string navigationProperty in navigationProperties)
            query = query.Include(navigationProperty);
        return query;
    }

    public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
    {
        var query = ApplyNavigation(dbSet.AsQueryable(), navigationProperties);
        T entity = await query.FirstOrDefaultAsync(predicate);
        return entity;
    }

    public async Task<List<T>> ListAsync()
    {
        return await dbSet.AsNoTracking().ToListAsync();
    }

    public Task<T> UpdateAsync(T updated)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
    {
        List<T> list;
        var query = ApplyNavigation(dbSet.AsQueryable(), navigationProperties);
        list = await query.Where(predicate).AsNoTracking().ToListAsync();
        return list;
    }
}
