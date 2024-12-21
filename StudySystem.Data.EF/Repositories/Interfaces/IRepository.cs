using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Add(T t);
        Task<T> AddAsyn(T t);
        bool Any();
        Task<bool> AnyAsync();
        int Count();
        Task<int> CountAsync();
        void Delete(T entity);
        Task<int> DeleteAsyn(T entity);
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        void Dispose();
#pragma warning restore S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        T Find(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        T Get(object id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsyn();
        Task<IEnumerable<T>> GetAllAsNoTrackingAsyn();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(params object[] id);
        void Save();
        Task<int> SaveAsync();
        T Update(T t, object key);
        Task<T> UpdateAsyn(T t, object key);
    }
}
