using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using FeroTech.Infrastructure.Repositories;
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
        private readonly IEmployeeRepository _rep;  
        public EmployeeController(ApplicationDbContext context, 
            IEmployeeRepository rep)
        {
            _context = context;
            _rep = rep;
        }

        public IActionResult Create()
        {
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                await _rep.Create(model);
            }
            return RedirectToAction("Create");
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