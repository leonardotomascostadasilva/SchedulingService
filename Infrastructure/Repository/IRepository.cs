using ServiceSchedule.Infrastructure.Entities;
using System.Linq.Expressions;

namespace ServiceSchedule.Infrastructure.Repository
{
    public interface IRepository
    {
        Task InsertAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity;
        Task<List<TEntity?>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity;
    }
}
