using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _rep;

        public EmployeeController(ApplicationDbContext context, IEmployeeRepository rep)
        {
            _context = context;
            _rep = rep;
        }

        // ---------- INDEX ----------
        public IActionResult Index()
        {
            return View();
        }

        // ---------- LOAD ALL (for DataTable AJAX) ----------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _rep.GetAllAsync();
            return Json(employees); // ✅ return plain array (DataTable expects array, not { data: [] })
        }

        // ---------- CREATE ----------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed." });
            }

            await _rep.Create(model);
            return Json(new { success = true, message = "Employee created successfully!" });
        }

        // ---------- EDIT ----------
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return Json(new { success = false, message = "Employee not found." });

            return Json(new { success = true, data = employee });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee model)
        {
            var employee = await _context.Employees.FindAsync(model.EmployeeId);
            if (employee == null)
                return Json(new { success = false, message = "Employee not found." });

            employee.FullName = model.FullName;
            employee.Email = model.Email;
            employee.Phone = model.Phone;
            employee.Department = model.Department;
            employee.JobTitle = model.JobTitle;
            employee.IsActive = model.IsActive;

            _context.Update(employee);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Employee updated successfully!" });
        }

        // ---------- DELETE ----------
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return Json(new { success = false, message = "Employee not found." });

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Employee deleted successfully!" });
        }
    }
}
