using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using FeroTech.Infrastructure.Services;  // ✅ add this
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly QRCodeService _qrCodeService;  // ✅ add dependency

        public AssetRepository(ApplicationDbContext context)
        {
            _context = context;
            _qrCodeService = new QRCodeService(context); // ✅ initialize service
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
            var asset = new Asset
            {
                AssetId = Guid.NewGuid(),
                Category = model.Category,
                Brand = model.Brand,
                Modell = model.Modell,
                PurchaseDate = model.PurchaseDate,
                PurchaseOrderNo = model.PurchaseOrderNo,
                Supplier = model.Supplier,
                PurchasePrice = model.PurchasePrice,
                WarrantyEndDate = model.WarrantyEndDate,
                Quantity = model.Quantity,
                Status = model.Status,
                Notes = model.Notes,
                IsActive = model.IsActive
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            // ✅ Call QR code generator service
            await _qrCodeService.GenerateAndSaveQRCodesAsync(asset);
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Assets.FindAsync(id);
            if (product != null)
                _context.Assets.Remove(product);

            await _context.SaveChangesAsync();
        }
    }
}
