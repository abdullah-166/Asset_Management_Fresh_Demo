using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<DistributedAsset?> GetByIdAsync(Guid id)
        {
            return await _context.DistributedAssets
                .FirstOrDefaultAsync(d => d.DistributedAssetId == id);
        }

        public async Task AddAsync(DistributedAsset distributedAsset)
        {
            _context.DistributedAssets.Add(distributedAsset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DistributedAsset distributedAsset)
        {
            _context.DistributedAssets.Update(distributedAsset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.DistributedAssets.FindAsync(id);
            if (entity != null)
            {
                _context.DistributedAssets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
