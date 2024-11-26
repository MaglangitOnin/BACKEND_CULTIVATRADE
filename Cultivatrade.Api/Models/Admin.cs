using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public Guid AdminId { get; set; }
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [StringLength(100)]
        public string Password { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }
    }
}
