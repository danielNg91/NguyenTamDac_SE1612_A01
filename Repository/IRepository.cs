using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository;
public interface IRepository<T>
{
    Task CreateAsync(T entity);
    public Task<IEnumerable<T>> GetAsync(
       Expression<Func<T, bool>> filter = null,
       int first = 0, int offset = 0,
       params string[] navigationProperties);
    Task<List<T>> ListAsync();
    Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
    Task<T> UpdateAsync(T updated);
    Task DeleteAsync(T entity);
}
