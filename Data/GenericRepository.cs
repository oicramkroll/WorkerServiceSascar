using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CarDbContext _context;
        private DbSet<T> _entities;
        public GenericRepository(
            CarDbContext context
            )
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public void delete(T entity)
        {
            _entities.Remove(entity);
        }

        public IQueryable<T> FromSQL(string query, params object[] paramiters)
        {
            return  _entities.FromSqlRaw(query, paramiters);
        }

        public IQueryable<T> getAll()
        {
            return _entities.AsQueryable();
        }

        public T getByPk(params object[] id)
        {
            return _entities.Find(id);
        }
        public async Task<T> getByPkAsync(params object[] id)
        {
            return await _entities.FindAsync(id);
        }

        public void save(T entity)
        {
            var local = getAll().FirstOrDefault();
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _entities.Add(entity);
        }
        public void saveRange(List<T> entity)
        {
            var local = getAll().FirstOrDefault();
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _entities.AddRange(entity);
        }

        public void update(T entity)
        {
            var local = getAll().FirstOrDefault();
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Modified;
            }
            _entities.Update(entity);
        }
    }
}
