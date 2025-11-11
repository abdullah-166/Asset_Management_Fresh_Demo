using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace FeroTech.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DistributedAssetController : Controller
    {
        private readonly IDistributedAssetRepository _distributedAssetRepo;
        private readonly IAssetRepository _assetRepo;
        private readonly IEmployeeRepository _employeeRepo;

        public DistributedAssetController(
            IDistributedAssetRepository distributedAssetRepo,
            IAssetRepository assetRepo,
            IEmployeeRepository employeeRepo)
        {
            _distributedAssetRepo = distributedAssetRepo;
            _assetRepo = assetRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            var assets = await _distributedAssetRepo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                // Filter locally — no foreign keys, so we filter by text match
                assets = assets.Where(a =>
                    a.EmployeeId.ToString().Contains(searchQuery) ||
                    a.AssetId.ToString().Contains(searchQuery) ||
                    (!string.IsNullOrEmpty(a.QRCodeValue) && a.QRCodeValue.ToLower().Contains(searchQuery)) ||
                    (!string.IsNullOrEmpty(a.Notes) && a.Notes.ToLower().Contains(searchQuery))
                );
            }

            ViewBag.SearchQuery = searchQuery;
            return View(assets);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var assets = await _assetRepo.GetAllAsync();
            ViewBag.AssetList = assets;
            var employees = await _employeeRepo.GetAllAsync();
            ViewBag.EmployeeList = employees;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DistributedAsset model)
        {
            if (!ModelState.IsValid)
            {
                var assets = await _assetRepo.GetAllAsync();
                ViewBag.AssetList = assets;
                return View(model);
            }

            await _distributedAssetRepo.AddAsync(model);
            TempData["Message"] = "Asset assigned successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _distributedAssetRepo.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DistributedAsset model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _distributedAssetRepo.UpdateAsync(model);
            TempData["Message"] = "Asset updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _distributedAssetRepo.DeleteAsync(id);
            TempData["Message"] = "Record deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
