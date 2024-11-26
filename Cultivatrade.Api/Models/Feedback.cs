using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Feedback")]
    [Index("BuyerId", Name = "IX_Feedback_BuyerId")]
    [Index("ProductId", Name = "IX_Feedback_OrderId")]
    public partial class Feedback
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BuyerId { get; set; }
        public double Rating { get; set; }
        public string? Message { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("Feedbacks")]
        public virtual User Buyer { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("Feedbacks")]
        public virtual Product Product { get; set; } = null!;
    }
}
