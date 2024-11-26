namespace Cultivatrade.Api.DTO
{
    public class MessageDTO_GET
    {
        public Guid MessageId { get; set; }
        public string BuyerFirstname { get; set; }
        public string BuyerLastname { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLastname { get; set; }
        public string SellerImage { get; set; }
        public string BuyerImage { get; set; }
        public string Message { get; set; }
        public Guid? BuyerId { get; set; }
        public Guid? SellerId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        
        
    }
    
    public class MessageDTO_POST
    {
        public string Message { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        
    }
}
