using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository;
public class Repository<T> : IRepository<T>
{
    public Task CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> ListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(T updated)
    {
        throw new NotImplementedException();
    }

    public Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties)
    {
        throw new NotImplementedException();
    }
}
