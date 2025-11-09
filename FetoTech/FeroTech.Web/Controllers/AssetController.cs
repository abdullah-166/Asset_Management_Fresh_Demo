using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Web.Controllers{
    [Authorize]
    public class AssetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAssetRepository _rep;

        public AssetController(ApplicationDbContext context, IAssetRepository rep)
        {
            _context = context;
            _rep = rep;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssetDto model)
        {
            if (ModelState.IsValid)
            {
                await _rep.Create(model);
                TempData["SuccessMessage"] = "Asset and QR Codes generated successfully!";
                return RedirectToAction("Create");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var assets = await _context.Assets.Where(x => x.Status == "Available").ToListAsync();
            return View(assets);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _rep.GetAllAsync();
            return Json(assets);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return Json(new { success = false, message = "Asset not found." });

            return Json(new { success = true, data = asset });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Asset model)
        {
            var asset = await _context.Assets.FindAsync(model.AssetId);
            if (asset == null)
                return Json(new { success = false, message = "Asset not found." });

            asset.Category = model.Category;
            asset.Brand = model.Brand;
            asset.Modell = model.Modell;
            asset.PurchasePrice = model.PurchasePrice;
            asset.Quantity = model.Quantity;
            asset.IsActive = model.IsActive;

            _context.Update(asset);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Asset updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return Json(new { success = false, message = "Asset not found." });

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Asset deleted successfully!" });
        }

    }
}
