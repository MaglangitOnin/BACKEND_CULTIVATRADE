using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cultivatrade.Api.Models;

namespace Cultivatrade.Api.DTO
{

    public class ProductDTO_GET
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLastname { get; set; }
        public string SellerEmail { get; set; }
        public string SellerPhone { get; set; }
        public string SellerAddress { get; set; }
        public string SellerImage { get; set; }
        public string ProductImage { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDisabled { get; set; }
        public double TotalSold { get; set; }
        public List<ProductFileDTO_GET> ProductFiles { get; set; }
        public List<FeedbackDTO_GET> Feedbacks { get; set; }
        public int NumberOfReport { get; set; }
        public List<BoostedProductDTO_GET> BoostExpiration { get; set; }
        public List<Order> Orders { get; set; }
        public bool? IsExpired { get; set; }
        public string FarmName { get; set; }
        public string FarmDescription { get; set; }
        public string FarmAddress { get; set; }
        public string BusinessPermitImage { get; set; }
    }
    public class ProductDTO_POST
    {
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CategoryName { get; set; }
    }
    
    public class ProductDTO_PUT
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CategoryName { get; set; }
    }
}
