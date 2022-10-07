using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BankingApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BankContext _context;
        public GenericRepository(BankContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            Remove(entity);
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}
