using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiffingAPITask.Data.EF;
using DiffingAPITask.Data.Entities;
using DiffingAPITask.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiffingAPITask.Data.Repositories
{
    public class EfRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected readonly ApplicationContext Context;
        protected readonly DbSet<T> DbSet;
        
        public EfRepository(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<T>();
        }
        
        public virtual async Task<T> FindAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            DbSet.Add(entity);
            await SaveAsync();
        }
        
        public virtual async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await SaveAsync();
        }

        public virtual async Task RemoveAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            DbSet.Remove(entity);
            await SaveAsync();
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await DbSet.AnyAsync(i => i.Id == id);
        }

        private async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}