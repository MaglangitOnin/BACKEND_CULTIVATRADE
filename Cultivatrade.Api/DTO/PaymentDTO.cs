using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cultivatrade.Api.DTO
{
    public class PaymentDTO_GET
    {
        public Guid PaymentId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public string PaymentSource { get; set; }
        public string PaymentCode { get; set; }
        public double TotalAmount { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string ProductFile { get; set; }
    }
}
