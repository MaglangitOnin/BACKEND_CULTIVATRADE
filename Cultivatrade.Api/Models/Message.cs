using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Message")]
    [Index("BuyerId", Name = "IX_Message_BuyerId")]
    [Index("SellerId", Name = "IX_Message_SellerId")]
    public partial class Message
    {
        [Key]
        public Guid MessageId { get; set; }
        public Guid? BuyerId { get; set; }
        public Guid? SellerId { get; set; }
        [Column("Message")]
        [StringLength(50)]
        public string Message1 { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("MessageBuyers")]
        public virtual User? Buyer { get; set; }
        [ForeignKey("SellerId")]
        [InverseProperty("MessageSellers")]
        public virtual User? Seller { get; set; }
    }
}
