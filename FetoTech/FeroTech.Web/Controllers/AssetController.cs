using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Web.Controllers
{
    public class AssetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAssetRepository _rep;
        private readonly INotificationRepository _notificationRepo; // <-- 1. Add notification repo

        // 2. Inject notification repo in the constructor
        public AssetController(ApplicationDbContext context, IAssetRepository rep, INotificationRepository notificationRepo)
        {
            _context = context;
            _rep = rep;
            _notificationRepo = notificationRepo; // <-- 3. Assign it
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

                // --- ADD NOTIFICATION ---
                string assetName = $"{model.Brand} {model.Modell}";
                await _notificationRepo.AddAsync(
                    message: $"New asset '{assetName}' ({model.Quantity}x) was created.",
                    module: "Asset",
                    actionType: "Create"
                );
                // --------------------------

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

            // --- ADD NOTIFICATION ---
            string assetName = $"{asset.Brand} {asset.Modell}";
            await _notificationRepo.AddAsync(
                message: $"Asset '{assetName}' was updated.",
                module: "Asset",
                actionType: "Update"
            );
            // --------------------------

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

            // --- ADD NOTIFICATION ---
            string assetName = $"{asset.Brand} {asset.Modell}";
            await _notificationRepo.AddAsync(
                message: $"Asset '{assetName}' was deleted.",
                module: "Asset",
                actionType: "Delete"
            );
            // --------------------------

            return Json(new { success = true, message = "Asset deleted successfully!" });
        }
    }
}