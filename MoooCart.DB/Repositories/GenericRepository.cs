using Microsoft.EntityFrameworkCore;
using MoooCart.DB.Base;
using MoooCart.DB.Contexts;
using MoooCart.lib.Exceptions;
using System.Linq.Expressions;


namespace MoooCart.DB.Repositories
{
    public class GenericRepository<T> (AppDbContext _Context) : IGenericRepository<T> where T : class
    {
        public async Task<int> AddAsync(T entity)
        {
            _Context.Set<T>().Add(entity);
            return await _Context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) throw new ProductNotFound($"Product '{id}' Not found");


            _Context.Set<T>().Remove(entity);
            return await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
        {
            IQueryable<T> query = _Context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _Context.Set<T>().AsNoTracking().Where(match).ToListAsync();
        }

        public async Task<T> GetByIDAsync(Guid id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) throw new ProductNotFound($"Product '{id}' Not found");
            return entity;
        }
       
        public async Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> match, params string[] includes)
        {
            IQueryable<T> query = _Context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(match);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _Context.Set<T>().Update(entity);
            return await _Context.SaveChangesAsync();

        }

        public IQueryable<T> GetQueryable()
        {
            return _Context.Set<T>().AsQueryable();
        }

    }
}
