using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FeroTech.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Department = model.Department,
                    JobTitle = model.JobTitle,
                    IsActive = model.IsActive
                };

                _context.Set<Employee>().Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Set<Employee>()
                .Where(e => e.IsActive)
                .ToListAsync();

            return View(employees);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await _context.Set<Employee>().FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Employee model)
        {
            if (id != model.EmployeeId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Set<Employee>().Any(e => e.EmployeeId == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Set<Employee>().FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _context.Set<Employee>().FindAsync(id);
            if (employee != null)
            {
                _context.Set<Employee>().Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}