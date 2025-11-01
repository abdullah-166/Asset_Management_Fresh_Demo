using FeroTech.Infrastructure.Application.DTOs;
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
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;
        public AssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets.FindAsync(id);
        }

        public async Task Create(AssetDto model)
        {
            var assets = new Asset
            {
                Category = model.Category,
                Brand = model.Brand,
                Modell = model.Modell,
                PurchaseDate = model.PurchaseDate,
                PurchaseOrderNo = model.PurchaseOrderNo,
                Supplier = model.Supplier,
                PurchasePrice = model.PurchasePrice,
                WarrantyEndDate = model.WarrantyEndDate,
                Status = model.Status,
                Notes = model.Notes,
                IsActive = model.IsActive
            };
            _context.Assets.Add(assets);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Assets.FindAsync(id);
            if (product != null) _context.Assets.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
