using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FeroTech.Web.Controllers
{

    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _rep;
        private readonly INotificationRepository _notificationRepo; // <-- 1. Add notification repo

        // 2. Inject notification repo in the constructor
        public EmployeeController(ApplicationDbContext context, IEmployeeRepository rep, INotificationRepository notificationRepo)
        {
            _context = context;
            _rep = rep;
            _notificationRepo = notificationRepo; // <-- 3. Assign it
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _rep.GetAllAsync();
            return Json(employees);
        }

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

            // --- ADD NOTIFICATION ---
            await _notificationRepo.AddAsync(
                message: $"New employee '{model.FullName}' was created.",
                module: "Employee",
                actionType: "Create"
            );
            // --------------------------

            return Json(new { success = true, message = "Employee created successfully!" });
        }

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

            // --- ADD NOTIFICATION ---
            await _notificationRepo.AddAsync(
                message: $"Employee '{employee.FullName}' was updated.",
                module: "Employee",
                actionType: "Update"
            );
            // --------------------------

            return Json(new { success = true, message = "Employee updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return Json(new { success = false, message = "Employee not found." });

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            // --- ADD NOTIFICATION ---
            await _notificationRepo.AddAsync(
                message: $"Employee '{employee.FullName}' was deleted.",
                module: "Employee",
                actionType: "Delete"
            );
            // --------------------------

            return Json(new { success = true, message = "Employee deleted successfully!" });
        }
    }
}