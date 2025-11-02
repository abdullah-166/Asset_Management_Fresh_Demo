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
        public AssetController(ApplicationDbContext context, IAssetRepository rep)
        {
            _context = context;
            _rep = rep;
        }
        
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
               TempData["SuccessMessage"] = "Successfully Created!";

            }
            return RedirectToAction("Create");
        }
        public async Task<IActionResult> Index()
        {
            var assets = await _context.Assets.Where(x => x.Status == "Available").ToListAsync();
            return View(assets);
        }
    }
}
