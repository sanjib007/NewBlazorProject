using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Extention;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories
{
    public interface IBaseRepository<T>
    {
        Task CreateAsync(T entity);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        T Creates(T entity);
        void Update(T entity);
        void UpdateLatest(T entity);
        Task UpdateRange(List<T> entryList);
        Task CreateListAsync(List<T> entity);
        Task Delete(T entity);
        Task DeleteRange(List<T> entryList);
        Task<IPagedList<T>> PaginateAsync(Expression<Func<T, bool>> expression, GeneralPaginationQuery paging = null);
        Task<int> SaveChangeAsync();
        int SaveChange();
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ChangeRequestDataContext _context;

        public BaseRepository(ChangeRequestDataContext context)
        {
            _context = context;
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            //await SaveChangeAsync();
        }
        public T Creates(T entity)
        {
            _context.Add(entity);
            SaveChange();
            return entity;
        }

        public virtual async Task CreateListAsync(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            //await SaveChangeAsync();
        }


        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                return await _context.Set<T>().ToListAsync();
            }

            return await _context.Set<T>().Where(expression).ToListAsync();
        }


        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }


        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            //SaveChange();
        }


        public virtual async Task UpdateRange(List<T> entryList)
        {
            _context.Set<T>().UpdateRange(entryList);
            //SaveChange();
        }

        public virtual void UpdateLatest(T entity)
        {
            _context.Set<T>().Update(entity);
            //SaveChange();
        }

        public virtual IQueryable<T> QueryAll(Expression<Func<T, bool>> expression)
        {
            return expression != null
                ? _context.Set<T>().AsQueryable().Where(expression)
                : _context.Set<T>().AsQueryable()
                    .AsNoTracking();
        }

        public virtual async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //SaveChange();
        }

        public async Task DeleteRange(List<T> entryList)
        {
            _context.Set<T>().RemoveRange(entryList);
            //SaveChange();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChange()
        {
            return _context.SaveChanges();
        }

        public virtual async Task<IPagedList<T>> PaginateAsync(Expression<Func<T, bool>> expression,
            GeneralPaginationQuery paginationQuery = null)
        {
            var query = QueryAll(expression);

            //query = query.OrderBy("Id DESC");

            var gg = query.ToSql();


            return await query.ToPagedListAsync(paginationQuery.PageNumber,
                paginationQuery.Pagination ? paginationQuery.PageSize : 10000);
        }
    }
}
