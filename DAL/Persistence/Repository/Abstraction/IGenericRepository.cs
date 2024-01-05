using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task Delete(T entity);
        Task UpdateAsync(T entity);
    }
}
