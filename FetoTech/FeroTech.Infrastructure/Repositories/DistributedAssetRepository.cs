using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Repositories
{
    public class DistributedAssetRepository : IDistributedAssetRepository
    {
        private readonly ApplicationDbContext _context;
        public DistributedAssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DistributedAsset>> GetAllAsync()
        {
            return await _context.DistributedAssets.ToListAsync();
        }

        public async Task<DistributedAsset?> GetByIdAsync(int id)
        {
            return await _context.DistributedAssets.FindAsync(id);
        }

        public async Task AddAsync(DistributedAsset asset)
        {
            _context.DistributedAssets.Add(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DistributedAsset asset)
        {
            _context.DistributedAssets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.DistributedAssets.FindAsync(id);
            if (product != null) _context.DistributedAssets.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
