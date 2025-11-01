using FeroTech.Infrastructure.Application.DTOs;
using FeroTech.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Application.Interfaces
{
    public interface IDistributedAssetRepository
    {
        Task<IEnumerable<DistributedAsset>> GetAllAsync();
        Task<DistributedAsset?> GetByIdAsync(int id);
        Task Create(DistributedAssetDto model);
        Task UpdateAsync(DistributedAsset product);
        Task DeleteAsync(int id);
    }
}
