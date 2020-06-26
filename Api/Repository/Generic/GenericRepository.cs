using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackgipInventory.Data;
using StackgipInventory.Entities.Shared;
using StackgipInventory.Helpers;

namespace StackgipInventory.Repository.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : Entity
    {
        private readonly ApplicationDbContext _dbContext;
     

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking().Where(a => a.IsDeleted != true);
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> where)
        {
            return await _dbContext.Set<TEntity>().Where(a=>a.IsDeleted != true).FirstOrDefaultAsync(where);
        }

        public async Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> where)
        {
            return await _dbContext.Set<TEntity>()
                .Where(where).ToListAsync();
        }

        public async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task Add(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(List<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }
        public IQueryable<TEntity> Query()
        {
            return _dbContext.Set<TEntity>().Where(a => a.IsDeleted == false)
                    .OrderByDescending(a => a.CreatedOn);
        }

        public async Task Update(TEntity entity)
        {
            entity.UpdatedOn = DateTime.UtcNow;
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        } 

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _dbContext.Set<TEntity>().Where(a => a.IsDeleted == false)
                .Where(where)
                .OrderByDescending(a => a.CreatedOn);
        }

        public async Task SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            _dbContext.Set<TEntity>().Update(entity);
        }
        public async Task<bool> Save()
        {
            return await ((DbContext)_dbContext).SaveChangesAsync(default(CancellationToken)) >= 0;
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);

        }

        public void UpdateRange(IList<TEntity> entities)
        {
            entities = entities.ForEach(x => x.UpdatedOn = DateTime.UtcNow).ToList();
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
