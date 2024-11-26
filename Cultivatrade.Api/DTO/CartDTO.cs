namespace Cultivatrade.Api.DTO
{
    public class CartDTO_GET
    {
        public Guid CartId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLastname { get; set; }
        public string SellerPhone { get; set; }
        public string SellerAddress { get; set; }
        public string SellerImage { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateTimeAdded { get; set; }

    }
    public class CartDTO_POST
    {
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
       
    }
}
