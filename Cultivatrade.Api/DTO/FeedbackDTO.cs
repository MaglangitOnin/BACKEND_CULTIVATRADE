using System.ComponentModel.DataAnnotations.Schema;

namespace Cultivatrade.Api.DTO
{
    public class FeedbackDTO_GET
    {
        public double Rating { get; set; }
        public string? Message { get; set; }
        public string BuyerName { get; set; }
        public string BuyerImage { get; set; }
        public DateTime DateTimeCreated { get; set; }

    }
    public class FeedbackDTO_POST
    {
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
