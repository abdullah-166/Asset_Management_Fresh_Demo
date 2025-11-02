using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Web.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IQRCodeRepository _rep;

        public QRCodeController(ApplicationDbContext context, IQRCodeRepository rep)
        {
            _context = context;
            _rep = rep;
        }

        // GET: Create QR Code
        public async Task<IActionResult> Create()
        {
            // Load all available assets for dropdown
            ViewBag.Assets = await _context.Assets
                .Select(a => new SelectListItem
                {
                    Value = a.AssetId.ToString(),
                    Text = $"{a.Brand} - {a.Modell}"
                })
                .ToListAsync();

            return View();
        }

        // POST: Create QR Code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QRCodeDto model)
        {
            if (ModelState.IsValid)
            {
                await _rep.Create(model);
                TempData["SuccessMessage"] = "QR Code created successfully!";
                return RedirectToAction(nameof(Create));
            }

            // Reload assets if validation fails
            ViewBag.Assets = await _context.Assets
                .Select(a => new SelectListItem
                {
                    Value = a.AssetId.ToString(),
                    Text = $"{a.Brand} - {a.Modell}"
                })
                .ToListAsync();

            return View(model);
        }

        // QR Code Index
        public async Task<IActionResult> Index()
        {
            var qrCodes = await _rep.GetAllAsync();
            return View(qrCodes);
        }
    }
}
