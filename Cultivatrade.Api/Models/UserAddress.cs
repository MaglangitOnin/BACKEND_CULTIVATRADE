using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Models
{
    [Table("UserAddress")]
    public partial class UserAddress
    {
        [Key]
        public Guid UserAddressId { get; set; }
        public Guid UserId { get; set; }
        public string Address { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserAddresses")]
        public virtual User User { get; set; } = null!;
    }
}
