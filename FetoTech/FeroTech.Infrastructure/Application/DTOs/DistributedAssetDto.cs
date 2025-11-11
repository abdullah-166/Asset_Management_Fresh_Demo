using System;
using System.ComponentModel.DataAnnotations;

namespace FeroTech.Infrastructure.Domain.Entities
{
    public class DistributedAssetDto
    {
        public Guid DistributedAssetId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        [StringLength(255)]
        public string? QRCodeValue { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }
    }
}
