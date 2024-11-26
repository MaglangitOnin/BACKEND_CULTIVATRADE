using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Notification")]
    [Index("ProductId", Name = "IX_Notification_ProductId")]
    [Index("UserId", Name = "IX_Notification_UserId")]
    public partial class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }
        public Guid? BuyerId { get; set; }
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public string Message { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Notifications")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Notifications")]
        public virtual User User { get; set; } = null!;
    }
}
