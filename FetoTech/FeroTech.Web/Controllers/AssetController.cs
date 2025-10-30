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
        public AssetController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Asset model)
        {
            if (ModelState.IsValid)
            {
                var asset = new Asset
                {
                    Name = model.Name,
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
                _context.Assets.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(model);
        }
        public async Task<IActionResult> Index()
        {
            var assets = await _context.Assets.Where(x => x.Status == "Available").ToListAsync();
            return View(assets);
        }
    }
}
