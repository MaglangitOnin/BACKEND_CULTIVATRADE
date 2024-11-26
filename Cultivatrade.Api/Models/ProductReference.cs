using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("ProductReference")]
    public partial class ProductReference
    {
        [Key]
        public Guid ProductReferenceId { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; } = null!;
        public double Price { get; set; }
        public string ProductImage { get; set; } = null!;
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }
    }
}
