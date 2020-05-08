using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.App.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T value);
        Task AddAsync(IEnumerable<T> value);
        Task RemoveAsync(T value);
        Task RemoveAsync(IEnumerable<T> value);
        Task UpdateAsync(T value);
    }
}
