using FeroTech.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Application.Interfaces
{
    public interface IQRCodeRepository
    {
        Task<IEnumerable<QRCode>> GetAllAsync();
        Task<QRCode?> GetByIdAsync(int id);
        Task AddAsync(QRCode product);
        Task UpdateAsync(QRCode product);
        Task DeleteAsync(int id);
    }
}