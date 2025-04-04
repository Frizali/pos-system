﻿using Microsoft.EntityFrameworkCore;
using pos_system.Data;

namespace pos_system.Repository
{
    public class CrudRepo<T> : ICrudRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public CrudRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync("System").ConfigureAwait(false);
        }

        public async Task AddRange(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities).ConfigureAwait(false);
            await _context.SaveChangesAsync("System").ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync("System");
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync("System");
        }
    }
}
