using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Application.DTOs
{
    public class DistributedAssetDto
    {
        public Guid DistributedId { get; set; }
        public int AssetId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public DateTime? AssignedAt { get; set; }
        [StringLength(128)]
        public string? QRCodeValue { get; set; }
        [StringLength(128)]
        public string Status { get; set; } = "Available";
        public DateTime? WarrantyEndDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
