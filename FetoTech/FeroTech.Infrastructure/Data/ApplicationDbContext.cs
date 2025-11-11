using FeroTech.Infrastructure.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeroTech.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<DistributedAsset> DistributedAssets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }
    }
}
