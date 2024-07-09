using System.Linq.Expressions;

namespace CoffeeShop.DataAccess.Common
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetFirstWhereAsync(Expression<Func<TEntity, bool>> match);
        Task UpdateAsync(TEntity entityToUpdate);
        Task<TEntity> InsertAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> FindAsync(Guid id);
        Task SaveChangesAsync();
        Task<List<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> match);
    }
}
