using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Payment")]
    public partial class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        [StringLength(50)]
        public string PaymentSource { get; set; } = null!;
        public string PayerId { get; set; } = null!;
        public string PaymentCodeId { get; set; } = null!;
        public double TotalAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }
    }
}
