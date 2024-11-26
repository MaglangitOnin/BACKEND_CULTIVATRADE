using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Product")]
    [Index("SellerId", Name = "IX_Product_SellerId")]
    public partial class Product
    {
        public Product()
        {
            BoostedProducts = new HashSet<BoostedProduct>();
            Carts = new HashSet<Cart>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            ProductFiles = new HashSet<ProductFile>();
            Reports = new HashSet<Report>();
        }

        [Key]
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDisabled { get; set; }

        [ForeignKey("SellerId")]
        [InverseProperty("Products")]
        public virtual User Seller { get; set; } = null!;
        [InverseProperty("Product")]
        public virtual ICollection<BoostedProduct> BoostedProducts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductFile> ProductFiles { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
