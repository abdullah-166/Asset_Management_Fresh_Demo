using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Application.DTOs
{
    public class QRCode
    {
       
        public Guid QRCodeId { get; set; }
        public Guid AssetId { get; set; }
        [StringLength(128)]
        public string? QRCodeValue { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public bool IsPrinted { get; set; } = false;
        public string? Notes { get; set; }
    }
}
