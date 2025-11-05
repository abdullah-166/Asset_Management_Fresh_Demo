using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Application.Interfaces;
using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FeroTech.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task Create(EmployeeDto model)
        {
            var assets = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Department = model.Department,
                JobTitle = model.JobTitle,
                IsActive = model.IsActive
            };
            _context.Employees.Add(assets);
            await _context.SaveChangesAsync();
        }
        

        public async Task UpdateAsync(Employee asset)
        {
            _context.Employees.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.DistributedAssets.FindAsync(id);
            if (product != null) _context.DistributedAssets.Remove(product);
            await _context.SaveChangesAsync();
        }

    }
}
