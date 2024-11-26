namespace Cultivatrade.Api.DTO
{
    public class ProductReferenceDTO_POST
    {
        public IFormFile? File { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductReferenceDTO_GET
    {
        public Guid ProductReferenceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ProductImage { get; set; }
        public string CategoryName { get; set; }

        public DateTime DateTimeCreated { get; set; }
    }
}
