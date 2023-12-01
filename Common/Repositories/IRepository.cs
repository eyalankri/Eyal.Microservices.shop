using System.Linq.Expressions;

namespace Common.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    }
}
