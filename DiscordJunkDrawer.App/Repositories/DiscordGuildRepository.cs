using DiscordJunkDrawer.App.Interfaces;
using DiscordJunkDrawer.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DiscordJunkDrawer.App.Repositories
{
    public class DiscordGuildRepository : IRepository<DiscordGuildModel>
    {
        private readonly StorageContext _context;

        public DiscordGuildRepository(StorageContext context)
        {
            _context = context;
        }

        public async Task<DiscordGuildModel> GetAsync(Expression<Func<DiscordGuildModel, bool>> predicate)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            return await queryable.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<DiscordGuildModel>> GetAllAsync()
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            return await queryable.ToListAsync();
        }

        public async Task<List<DiscordGuildModel>> FindAsync(Expression<Func<DiscordGuildModel, bool>> predicate)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            return await queryable.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(DiscordGuildModel value)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            var alreadyAdded = await queryable.AnyAsync(x => x.Id == value.Id);

            if (!alreadyAdded)
            {
                await _context.DiscordGuilds.AddAsync(value);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAsync(IEnumerable<DiscordGuildModel> value)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();

            foreach (var item in value)
            {
                var alreadyAdded = await queryable.AnyAsync(x => x.Id == item.Id);
                if (!alreadyAdded)
                {
                    await _context.DiscordGuilds.AddAsync(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(DiscordGuildModel value)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            var storedLocally = await queryable.AnyAsync(x => x.Id == value.Id);
            if (storedLocally)
            {
                _context.DiscordGuilds.Remove(value);
                await _context.SaveChangesAsync();
            }

        }

        public async Task RemoveAsync(IEnumerable<DiscordGuildModel> value)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();

            foreach (var item in value)
            {
                var storedLocally = await queryable.AnyAsync(x => x.Id == item.Id);
                if (storedLocally)
                {
                     _context.DiscordGuilds.Remove(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiscordGuildModel value)
        {
            var queryable = _context.DiscordGuilds.Include(x => x.Roles).AsQueryable();
            var result = await queryable.FirstOrDefaultAsync(x => x.Id == value.Id);
            if (result != null)
            {
                result = value;
                await _context.SaveChangesAsync();
            }
        }
    }
}
