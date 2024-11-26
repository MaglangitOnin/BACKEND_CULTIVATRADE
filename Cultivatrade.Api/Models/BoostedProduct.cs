using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("BoostedProduct")]
    [Index("ProductId", Name = "IX_BoostedProduct_ProductId")]
    public partial class BoostedProduct
    {
        [Key]
        public Guid BoostedProductId { get; set; }
        public Guid ProductId { get; set; }
        public double BoostCost { get; set; }
        public int NumberOfDays { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("BoostedProducts")]
        public virtual Product Product { get; set; } = null!;
    }
}
