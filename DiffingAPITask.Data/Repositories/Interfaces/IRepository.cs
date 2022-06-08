using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiffingAPITask.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(int id);
        Task<bool> Exists(int id);
    }
}