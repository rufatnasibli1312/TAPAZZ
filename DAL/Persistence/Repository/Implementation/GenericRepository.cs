using DAL.Data;
using DAL.Persistence.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Persistence.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public MyDbContext _context { get; }
        private readonly DbSet<T> _table;
        public GenericRepository(MyDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
            _context.SaveChanges();
        }

        public async Task Delete(T entity)
        {

            _table.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            var t = await _table.FindAsync(id);
            return t;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
           await _context.SaveChangesAsync();
           
        }
    }
}
