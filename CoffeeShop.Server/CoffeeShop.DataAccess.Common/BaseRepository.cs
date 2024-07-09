using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CoffeeShop.DataAccess.Common
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbContext context;

        protected BaseRepository(DbContext context) => this.context = context;

        public virtual async Task<List<TEntity>> GetAllAsync() => await context.Set<TEntity>().AsNoTracking().ToListAsync();

        public virtual async Task<TEntity> GetFirstWhereAsync(Expression<Func<TEntity, bool>> match) => await context.Set<TEntity>().FirstOrDefaultAsync(match);

        public virtual async Task<List<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> match) => await context.Set<TEntity>().Where(match).ToListAsync();

        public virtual async Task<TEntity> FindAsync(Guid id) => await context.Set<TEntity>().FindAsync(id);

        public virtual async Task UpdateAsync(TEntity entityToUpdate)
        {
            try
            {
                var entry = context.Entry(entityToUpdate);

                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Modified;
                }

                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                int affectedRows = await context.SaveChangesAsync();

                if (affectedRows > 0)
                {
                    Console.WriteLine("Save operations completed successfully");
                }
                else
                {
                    Console.WriteLine("No changes to save");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error during save operations - {ex.Message}");
                throw;
            }
        }

        public void IncludeNavigationProperties(TEntity entityToUpdate)
        {
            var entry = context.Entry(entityToUpdate);

            try
            {
                foreach (var navigationEntry in entry.Navigations)
                {
                    if (navigationEntry.CurrentValue == null)
                    {
                        navigationEntry.Load();
                    }

                    if (navigationEntry.CurrentValue == null)
                    {
                        continue;
                    }

                    if (navigationEntry is ReferenceEntry referenceEntry)
                    {
                        var referencedEntry = context.Entry(referenceEntry.CurrentValue);
                        if (referencedEntry.State == EntityState.Detached)
                        {
                            referencedEntry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        var collectionEntry = (CollectionEntry)navigationEntry;
                        if (collectionEntry.CurrentValue != null)
                        {
                            var items = (IEnumerable<object>)collectionEntry.CurrentValue;
                            var detachedItems = items.Where(item => item != null && context.Entry(item).State == EntityState.Detached);
                            foreach (var item in detachedItems)
                            {
                                context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            try
            {
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();

                return entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
