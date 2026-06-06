using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params string[] includes);
        Task<T> GetByIDAsync(Guid Id);
        Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> match, params string[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
        IQueryable<T> GetQueryable();
    }
}
