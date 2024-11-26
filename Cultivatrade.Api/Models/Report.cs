using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Report")]
    [Index("BuyerId", Name = "IX_Report_BuyerId")]
    [Index("ProductId", Name = "IX_Report_ProductId")]
    public partial class Report
    {
        [Key]
        public Guid ReportId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public string Reason { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("Reports")]
        public virtual User Buyer { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("Reports")]
        public virtual Product Product { get; set; } = null!;
    }
}
