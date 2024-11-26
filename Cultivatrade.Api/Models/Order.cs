using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Order")]
    [Index("BuyerId", Name = "IX_Order_BuyerId")]
    [Index("ProductId", Name = "IX_Order_ProductId")]
    [Index("SellerId", Name = "IX_Order_SellerId")]
    public partial class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityBought { get; set; }
        public double TotalAmount { get; set; }
        [StringLength(50)]
        public string PaymentOption { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrderDateReceived { get; set; }
        [StringLength(50)]
        public string OrderStatus { get; set; } = null!;
        [StringLength(50)]
        public string? DeliveryAddress { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; }
        [StringLength(50)]
        public string? DeliveryCourier { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("OrderBuyers")]
        public virtual User Buyer { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("Orders")]
        public virtual Product Product { get; set; } = null!;
        [ForeignKey("SellerId")]
        [InverseProperty("OrderSellers")]
        public virtual User Seller { get; set; } = null!;
    }
}
