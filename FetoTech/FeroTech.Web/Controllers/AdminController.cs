using FeroTech.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FeroTech.Web.Controllers{
    public class AdminController : Controller{
        private readonly AdminRepository _adminRepository;

        public AdminController(AdminRepository adminRepository){
            _adminRepository = adminRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password){
            var admin = await _adminRepository.LoginAsync(email, password);

            if (admin != null){
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, admin.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [Authorize]
        public IActionResult Dashboard(){
            ViewBag.AdminEmail = User.Identity?.Name;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
