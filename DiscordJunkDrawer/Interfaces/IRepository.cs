using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAsync();
        Task<List<T>> FindAsync(Predicate<T> predicate);
        Task AddAsync(T value);
        Task AddAsync(IEnumerable<T> value);
        Task RemoveAsync(IEnumerable<T> value);
        Task UpdateAsync(T value);
    }
}
