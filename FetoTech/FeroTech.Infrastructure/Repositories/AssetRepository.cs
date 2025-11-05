using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using QRCoder;
using System.IO;

using QRCodeEntity = FeroTech.Infrastructure.Domain.Entities.QRCode;

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

            var qrCodes = new List<QRCodeEntity>();
            for (int i = 1; i <= asset.Quantity; i++)
            {
                string qrValue = $"{asset.AssetId}-{Guid.NewGuid()}";

                qrCodes.Add(new QRCodeEntity
                {
                    QRCodeId = Guid.NewGuid(),
                    AssetId = asset.AssetId,
                    QRCodeValue = qrValue,
                    GeneratedAt = DateTime.UtcNow,
                    Notes = $"QR Code {i} for {asset.Brand} {asset.Modell}"
                });
            }

            _context.QRCodes.AddRange(qrCodes);
            await _context.SaveChangesAsync();

            var pdfBytes = await GenerateQRCodePdfAsync(qrCodes);

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, $"{asset.Brand}_{asset.Modell}_QRCodes.pdf");
            await File.WriteAllBytesAsync(filePath, pdfBytes);
        }

        private async Task<byte[]> GenerateQRCodePdfAsync(IEnumerable<QRCodeEntity> qrCodes)
        {
            using var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            int x = 50, y = 50;

            foreach (var qr in qrCodes)
            {
                using var generator = new QRCodeGenerator();
                var qrData = generator.CreateQrCode(qr.QRCodeValue, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrData);
                var qrBytes = qrCode.GetGraphic(10);

                using var ms = new MemoryStream(qrBytes);
                var img = XImage.FromStream(() => ms);

                
                gfx.DrawImage(img, x, y, 100, 100);

             
                var font = new XFont("Arial", 9, XFontStyle.Regular);
                var textRect = new XRect(x, y + 110, 180, 80);

                var readableText = qr.QRCodeValue.Replace("\n", Environment.NewLine);
                gfx.DrawString(readableText, font, XBrushes.Black, textRect, XStringFormats.TopLeft);

              
                x += 200; 

                if (x > 400) 
                {
                    x = 50;
                    y += 200; 
                    if (y > 700)
                    {
                        page = doc.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 50;
                    }
                }
            }


            using var stream = new MemoryStream();
            doc.Save(stream, false);
            return await Task.FromResult(stream.ToArray());
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
