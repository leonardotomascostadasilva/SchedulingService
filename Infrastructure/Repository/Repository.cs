using Microsoft.EntityFrameworkCore;
using ServiceSchedule.Infrastructure.Entities;
using System.Linq.Expressions;

namespace ServiceSchedule.Infrastructure.Repository
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

            return entity;
        }

        public async Task<List<TEntity?>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            var entities = await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
            return entities;
        }

        public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
