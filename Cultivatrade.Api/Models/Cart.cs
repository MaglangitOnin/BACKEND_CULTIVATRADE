using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Cart")]
    [Index("BuyerId", Name = "IX_Cart_BuyerId")]
    [Index("ProductId", Name = "IX_Cart_ProductId")]
    public partial class Cart
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("Carts")]
        public virtual User Buyer { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("Carts")]
        public virtual Product Product { get; set; } = null!;
    }
}
