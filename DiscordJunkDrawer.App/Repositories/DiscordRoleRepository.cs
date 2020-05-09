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
    public class DiscordRoleRepository : IRepository<DiscordRoleModel>
    {
        private readonly StorageContext _context;

        public DiscordRoleRepository(StorageContext context)
        {
            _context = context;
        }

        public async Task<DiscordRoleModel> GetAsync(Expression<Func<DiscordRoleModel, bool>> predicate)
        {
            return await _context.DiscordRoles.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<DiscordRoleModel>> GetAllAsync()
        {
            return await _context.DiscordRoles.AsQueryable().ToListAsync();
        }

        public async Task<List<DiscordRoleModel>> FindAsync(Expression<Func<DiscordRoleModel, bool>> predicate)
        {
            return await _context.DiscordRoles.AsQueryable().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(DiscordRoleModel value)
        {
            var alreadyAdded = await _context.DiscordRoles.AsQueryable().AnyAsync(x => x.Id == value.Id && x.GuildId == value.GuildId);
            if (!alreadyAdded)
            {
                await _context.DiscordRoles.AddAsync(value);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAsync(IEnumerable<DiscordRoleModel> value)
        {
            foreach(var item in value)
            {
                var alreadyAdded = await _context.DiscordRoles.AsQueryable().AnyAsync(x => x.Id == item.Id && x.GuildId == item.GuildId);
                if (!alreadyAdded)
                {
                    await _context.DiscordRoles.AddAsync(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(DiscordRoleModel value)
        {
            var storedLocally = await _context.DiscordRoles.AsQueryable().AnyAsync(x => x.Id == value.Id && x.GuildId == value.GuildId);
            if (storedLocally)
            {
                _context.DiscordRoles.Remove(value);
                await _context.SaveChangesAsync();
            }

        }

        public async Task RemoveAsync(IEnumerable<DiscordRoleModel> value)
        {
            foreach (var item in value)
            {
                var storedLocally = await _context.DiscordRoles.AsQueryable().AnyAsync(x => x.Id == item.Id && x.GuildId == item.GuildId);
                if (storedLocally)
                {
                     _context.DiscordRoles.Remove(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DiscordRoleModel value)
        {
            var result = await _context.DiscordRoles.AsQueryable().FirstOrDefaultAsync(x => x.Id == value.Id && x.GuildId == value.GuildId);
            if (result != null)
            {
                result = value;
                await _context.SaveChangesAsync();
            }
        }
    }
}
