using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace FeroTech.Web.Controllers
{
    public class DistributedAssetController : Controller{
        private readonly ApplicationDbContext _context;
        private readonly IDistributedAssetRepository _rep;
        public DistributedAssetController(ApplicationDbContext context, 
            IDistributedAssetRepository rep){
            _context = context;
            _rep = rep;
        }
        public IActionResult Index(){
            return View();
        }
        public IActionResult Create(){
            return View();
        }
        public async Task<IActionResult> Create(DistributedAssetDto model) {
            if (ModelState.IsValid){
                await _rep.Create(model);
            }
            return RedirectToAction("Create");
        }
    }
}
