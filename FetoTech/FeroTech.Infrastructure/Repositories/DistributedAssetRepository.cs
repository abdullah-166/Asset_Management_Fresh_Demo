using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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

        public async Task Create(DistributedAssetDto asset){
            var distributedAsset = new DistributedAsset
            {
               AssetId= asset.AssetId,
                AssignedToUserId= asset.AssignedToUserId,
                AssignedAt= asset.AssignedAt,
                QRCodeValue= asset.QRCodeValue, 
                Status= asset.Status,
                WarrantyEndDate= asset.WarrantyEndDate,
                Notes= asset.Notes,
                IsActive= asset.IsActive
            };
            _context.DistributedAssets.Add(distributedAsset);
            await _context.SaveChangesAsync();
        }
        public async Task<Dictionary<string, object>> GetCreateViewDataAsync()
        {
            var availableAssets = await _context.Assets
                .Where(a => a.Status == "Available")
                .Select(a => new { a.AssetId })
                .ToListAsync();

            var activeEmployees = await _context.Employees
                .Where(e => e.IsActive)
                .Select(e => new { e.EmployeeId, e.FullName })
                .ToListAsync();

            return new Dictionary<string, object>
    {
        { "AvailableAssets", availableAssets },
        { "ActiveEmployees", activeEmployees }
    };
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
