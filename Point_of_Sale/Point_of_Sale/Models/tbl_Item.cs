﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Models
{
    public class tbl_Item
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }

        public int BrandId { get; set; }

        public int SupplierId { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public DateTime? DateExpired { get; set; }
    }
}
