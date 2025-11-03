using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Web.Controllers
{
    public class DistributedAssetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedAssetRepository _rep;

        public DistributedAssetController(ApplicationDbContext context,
            IDistributedAssetRepository rep)
        {
            _context = context;
            _rep = rep;
        }
        public async Task<IActionResult> Index()
        {
            var distributedAssets = await _rep.GetAllAsync();
            return View(distributedAssets);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var data = await _rep.GetCreateViewDataAsync();

            ViewBag.AvailableAssets = data["AvailableAssets"];
            ViewBag.ActiveEmployees = data["ActiveEmployees"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistributedAssetDto model)
        {
            if (ModelState.IsValid)
            {
                await _rep.Create(model);
                return RedirectToAction(nameof(Index));
            }

            var data = await _rep.GetCreateViewDataAsync();
            ViewBag.AvailableAssets = data["AvailableAssets"];
            ViewBag.ActiveEmployees = data["ActiveEmployees"];

            return View(model);
        }
    }
}
//This is here to check the C# language version for the code file.