using Microsoft.AspNetCore.Mvc;

namespace FeroTech.Web.Controllers
{
    public class DistributedAssetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
