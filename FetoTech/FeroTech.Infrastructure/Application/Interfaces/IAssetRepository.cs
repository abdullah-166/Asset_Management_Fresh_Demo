using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Application.Interfaces
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(int id);
        Task Create(AssetDto product);
        Task UpdateAsync(Asset product);
        Task DeleteAsync(int id);
    }
}
