namespace Cultivatrade.Api.DTO
{
    public class ProductFileDTO_POST
    {
        public Guid ProductId { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class ProductFileDTO_GET
    {
        public Guid ProductFileId { get; set; }
        public string ProductPath { get; set; }
        public string ProductImage { get; set; }
    }
}
