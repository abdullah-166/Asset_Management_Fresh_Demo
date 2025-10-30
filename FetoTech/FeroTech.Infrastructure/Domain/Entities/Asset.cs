﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeroTech.Infrastructure.Domain.Entities
{
    public class Asset
    {
        [Key]
        public Guid AssetId { get; set; }
        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Brand { get; set; }

        [StringLength(50)]
        public string? Modell { get; set; }

        public DateTime? PurchaseDate { get; set; }

        [StringLength(50)]
        public string? PurchaseOrderNo { get; set; }//invoice number

        [StringLength(100)]
        public string? Supplier { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PurchasePrice { get; set; }

        public DateTime? WarrantyEndDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Available";

        [StringLength(128)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
