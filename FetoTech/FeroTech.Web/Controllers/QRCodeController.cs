using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FeroTech.Web.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QRCodeController(ApplicationDbContext context)
        {
            _context = context;
        }
        //iiii
        // GET: QRCode/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QRCode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QRCode model)
        {
            if (ModelState.IsValid)
            {
                var qrCode = new QRCode
                {
                    QRCodeId = Guid.NewGuid(),
                    AssetId = model.AssetId,
                    QRCodeValue = model.QRCodeValue,
                    Quantity = model.Quantity,
                    GeneratedAt = DateTime.UtcNow,
                    IsPrinted = model.IsPrinted,
                    Notes = model.Notes
                };

                _context.QRCodes.Add(qrCode);
                await _context.SaveChangesAsync();

                // Redirect to index or show success message
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: QRCode/Index
        public async Task<IActionResult> Index()
        {
            var qrCodes = await _context.QRCodes
                .Include(q => q.AssetId) // optional — replace with navigation property when available
                .OrderByDescending(q => q.GeneratedAt)
                .ToListAsync();

            return View(qrCodes);
        }

        // GET: QRCode/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var qrCode = await _context.QRCodes.FirstOrDefaultAsync(q => q.QRCodeId == id);

            if (qrCode == null)
                return NotFound();

            return View(qrCode);
        }

        // GET: QRCode/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var qrCode = await _context.QRCodes.FirstOrDefaultAsync(q => q.QRCodeId == id);

            if (qrCode == null)
                return NotFound();

            return View(qrCode);
        }

        // POST: QRCode/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var qrCode = await _context.QRCodes.FindAsync(id);
            if (qrCode != null)
            {
                _context.QRCodes.Remove(qrCode);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
