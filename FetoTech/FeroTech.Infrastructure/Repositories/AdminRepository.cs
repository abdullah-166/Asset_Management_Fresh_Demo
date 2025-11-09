using FeroTech.Infrastructure.Data;
using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Infrastructure.Repositories{
    public class AdminRepository{
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context){
            _context = context;
        }

        public async Task InitializeAdminAsync(){
            if (!await _context.Admins.AnyAsync()){
                var admin = new Admin{
                    Email = "admin@example.com",
                    Password = "Admin@123",
                    CreatedDate = DateTime.UtcNow
                };
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Admin?> LoginAsync(string email, string password){
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
