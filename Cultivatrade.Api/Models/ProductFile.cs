using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("ProductFile")]
    [Index("ProductId", Name = "IX_ProductFile_ProductId")]
    public partial class ProductFile
    {
        [Key]
        public Guid ProductFileId { get; set; }
        public Guid ProductId { get; set; }
        [StringLength(50)]
        public string Image { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductFiles")]
        public virtual Product Product { get; set; } = null!;
    }
}
