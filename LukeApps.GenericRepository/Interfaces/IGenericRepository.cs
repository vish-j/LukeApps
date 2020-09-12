using System;
using System.Linq;
using System.Linq.Expressions;

namespace LukeApps.GenericRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(string includeProperties);

        T FindBy(Expression<Func<T, bool>> predicate, string includeProperties);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void SaveChanges();
    }
}