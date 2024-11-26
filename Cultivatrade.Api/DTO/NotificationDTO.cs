namespace Cultivatrade.Api.DTO
{
    public class NotificationDTO_POST
    {
        public Guid BuyerId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? OrderId { get; set; }
        public string Message { get; set; }
    }

    public class NotificationDTO_GET
    {
        public Guid SellerId { get; set; }
        public Guid? BuyerId { get; set; }
        public Guid NotificationId { get; set; }
        public Guid? ProductId { get; set; }
        public string Message { get; set; }
        public string? ProductName { get; set; }
        public int? QuantityBought { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string BuyerFirstname { get; set; }
        public string BuyerLastname { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLastname { get; set; }
        public string OrderStatus { get; set; }
        public string BuyerImage { get; set; }
        public string SellerImage { get; set; }

    }
}
