namespace Cultivatrade.Api.DTO
{
    public class OrderDTO_GET
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int QuantityBought { get; set; }
        public double TotalAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryCourier { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLastname { get; set; }
        public string SellerPhone { get; set; }
        public string SellerAddress { get; set; }

        public string BuyerFirstname { get; set; }
        public string BuyerLastname { get; set; }
        public string PaymentOption { get; set; }
        public string BuyerImage { get; set; }
        public string SellerImage { get; set; }
        public string ProductImage { get; set; }
        public string OrderStatus { get; set; }
        public double TotalSold { get; set; }
        public double Quantity { get; set; }

    }
    public class OrderDTO_POST
    {
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ProductId { get; set; }
        public string PaymentOption { get; set; } 
        public int QuantityBought { get; set; }
        public float TotalAmount { get; set; }
        public string DeliveryAddress { get; set; }
        public string? PaymentSource { get; set; }
        public string? PayerId { get; set; }
        public string? PaymentCodeId { get; set; }

    }

    public class OrderDTO_PUT
    {
        public string OrderStatus { get; set; }
        public DateTime? OrderDateReceived { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryCourier { get; set; }
    }
}
