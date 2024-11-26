namespace Cultivatrade.Api.DTO
{
    public class ReportDTO_GET
    {
        public string Reason { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string BuyerFirstname { get; set; }
        public string BuyerLastname { get; set; }

    }

    public class ReportDTO_POST
    {
        public Guid ProductId { get; set; }
        public Guid BuyerId { get; set; }
        public string Reason { get; set; }
    }
}
