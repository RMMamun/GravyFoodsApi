using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

//*** It's a common reposity as for Generic Repository model 
namespace GravyFoodsApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MasjidDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MasjidDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
