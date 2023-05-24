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
    Task<List<T>> ListAsync();
    Task<T> FindByIdAsync(int id);
    Task<IList<T>> WhereAsync(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<T> UpdateAsync(T updated);
    Task DeleteAsync(int id);
}
