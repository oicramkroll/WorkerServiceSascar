using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> getAll();
        IQueryable<T> FromSQL(string query, params object[] paramiters);
        T getByPk(params object[] id);
        Task<T> getByPkAsync(params object[] id);
        void save(T entity);
        void update(T entity);
        void delete(T entity);


    }
}
