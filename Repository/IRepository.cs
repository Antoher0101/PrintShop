using PrintShop.core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintShop.Repository
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> Items { get; }
        T Get(int id);
        Task<T> GetAsync(int id, CancellationToken Cancel = default);
        T Add(T item);
        Task<T> AddAsync(T item, CancellationToken Cancel = default);
        T Update(T item);
        Task<T> UpdateAsync(T item, CancellationToken Cancel = default);
        T Delete(int id);
        Task<T> DeleteAsync(int id, CancellationToken Cancel = default);
    }
}